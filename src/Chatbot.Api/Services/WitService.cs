using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Chatbot.Api.Models;

namespace Chatbot.Api.Services
{
    public class WitService
    {
        private AiSettings _aiSettings;

        public WitService(IOptions<AiSettings> aiSettings)
        {
            _aiSettings = aiSettings.Value;
        }

        public async Task<string> ConverseAsync(string message, Func<string, Dictionary<string, string>> getContext)
        {
            var messageId = Guid.NewGuid().ToString();
            var converseResponse = await ConverseAsync(messageId, message, null).ConfigureAwait(false);

            while (converseResponse != null && converseResponse.TypeCode != WitResponseTypes.Stop)
            {
                switch (converseResponse.TypeCode)
                {
                    case WitResponseTypes.Merge:
                        converseResponse = null;
                        break;
                    case WitResponseTypes.Message:
                        return converseResponse.msg;
                    case WitResponseTypes.Action:
                        var context = getContext != null ? getContext(converseResponse.action) : null;
                        converseResponse = await ConverseAsync(messageId, message, context).ConfigureAwait(false);
                        break;
                    case WitResponseTypes.Error:
                        converseResponse = null;
                        break;
                    default:
                        break;
                }                
            }

            return null;
        }

        private async Task<WitConverseResponse> ConverseAsync(string sessionId, string message, Dictionary<string, string> context)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_aiSettings.BaseUri) })
            {
                try
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aiSettings.Token);

                    var url = "/converse?v=" + _aiSettings.Version + "&session_id=" + sessionId;
                    if (!string.IsNullOrWhiteSpace(message))
                        url += "&q=" + message;

                    HttpContent httpContent = null;
                    if(context != null)
                    {
                        var contextInString = JsonConvert.SerializeObject(context);
                        httpContent = new StringContent(contextInString, Encoding.UTF8, "application/json");
                    }
                    var response = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }           
                                    
                    var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<WitConverseResponse>(stringResponse);                
                }
                catch (Exception)
                {
                    return null;
                }               
            }          
        }

        private async Task<WitMessage> GetMessageAsync(string message, string messageId, string threadId)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_aiSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aiSettings.Token);

                var url = "/message?v=" + _aiSettings.Version + "&q=" + message;
                if (!string.IsNullOrWhiteSpace(messageId))
                    url += "&msg_id=" + messageId;
                if (!string.IsNullOrWhiteSpace(threadId))
                    url += "&thread_id=" + threadId;

                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var witMessage = JsonConvert.DeserializeObject<WitMessage>(stringResponse);

                if (witMessage == null || !string.IsNullOrWhiteSpace(witMessage.error))
                    return null;

                return witMessage;
            }
        }

    }
}
