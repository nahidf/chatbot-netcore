using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Chatbot.Api.Controllers
{
    public class TwilioService
    {
        private SmsSettings _smsSettings;

        public TwilioService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        public async Task<bool> SendSmsAsync(string number, string message)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_smsSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_smsSettings.Sid}:{_smsSettings.Token}")));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("To",$"+{number}"),
                    new KeyValuePair<string, string>("From", _smsSettings.From),
                    new KeyValuePair<string, string>("Body", message)
                });

                var response = await client.PostAsync(_smsSettings.RequestUri, content).ConfigureAwait(false);
               
                var result = response.Content.ReadAsStringAsync();
                //log the result 

                return response.IsSuccessStatusCode;
            }
        }
    }
}
