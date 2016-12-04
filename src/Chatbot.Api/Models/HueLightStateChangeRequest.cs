using Chatbot.Api.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Chatbot.Api.Models
{
    public class HueLightStateChangeRequest
    {
        private List<int> hueColors = new List<int>(new int[] { 0, 12750, 25500, 46920, 56100, 65280 });
        private List<int> hueBrightness = new List<int>(new int[] { 100, 150, 200, 254 });

        public HueLightStateChangeRequest(string lightName, string statusValue)
        {
            var lightId = lightName == "outside" ? "2" : "3";
            var status = statusValue == "on" ? true : false;

            LightId = lightId;
            on = status;

            if(status)
            {
                hue = status ? (int?)hueColors.Shuffle().First() : null;
                bri = status ? (int?)hueBrightness.Shuffle().First() : null;
            }
        }

        [JsonIgnore]
        public string LightId { get; private set; }

        public int? hue { get; set; }

        public bool? on { get; set; }

        public int? bri { get; set; }

        public string effect { get; set; }
    }
}