using System.Collections.Generic;

namespace Chatbot.Api.Models
{
    public class WitMessage 
    {
        public string error { get; set; }

        public string code { get; set; }
        
        public string msg_id { get; set; }
        
        public string _text { get; set; }
        
        public Dictionary<string, List<WitEntity>> entities { get; set; }
    }

}