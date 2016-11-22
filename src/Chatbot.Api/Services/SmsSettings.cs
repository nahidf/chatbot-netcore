// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chatbot.Api.Controllers
{
    public class SmsSettings
    {
        public string Sid { get; set; }
        public string Token { get; set; }
        public string BaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
    }
}
