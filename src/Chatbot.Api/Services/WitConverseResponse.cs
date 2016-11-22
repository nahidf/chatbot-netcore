using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chatbot.Api.Controllers
{
    public class WitConverseResponse
    {
        public string type { get; set; }
        
        public string msg { get; set; }

        public string action { get; set; }
        
        public Dictionary<string, List<WitEntity>> entities { get; set; }
        
        public double confidence { get; set; }      

        [JsonIgnore]
        public WitResponseTypes TypeCode
        {
            get
            {
                switch (type)
                {
                    case "merge":
                        return WitResponseTypes.Merge;
                    case "msg":
                        return WitResponseTypes.Message;
                    case "action":
                        return WitResponseTypes.Action;
                    case "stop":
                        return WitResponseTypes.Stop;
                    default:
                        return WitResponseTypes.Error;
                }
            }
        }
    }

}