using Chatbot.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Api.Services
{
    public class HueService
    {
        private HueSettings _settings;

        public HueService(IOptions<HueSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SetLightState(HueLightStateChangeRequest request)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_settings.BaseUri) })
            {
                var url = _settings.Username + "/lights/"+ request.LightId + "/state";

                var contextInString = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(contextInString, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, httpContent).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<HueLight> GetLightStatus(string lightId)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_settings.BaseUri) })
            {
                var url = _settings.Username + "/lights/" + lightId;

                var response = await client.GetAsync(url).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<HueLight>(stringResponse);
            }
        }
    }
}
