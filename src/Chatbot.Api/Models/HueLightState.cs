namespace Chatbot.Api.Models
{

    public class HueLightState
    {
        public string Status
        {
            get
            {
                return on ? "On" : "Off";
            }
        }

        public bool on { get; set; }
    }
}
