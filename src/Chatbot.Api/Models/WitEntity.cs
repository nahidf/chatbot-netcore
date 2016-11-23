using Newtonsoft.Json.Linq;

namespace Chatbot.Api.Models
{
    public class WitEntity
    {
        public string metadata { get; set; }

        public JToken value { get; set; }

        public double confidence { get; set; }
    }

}