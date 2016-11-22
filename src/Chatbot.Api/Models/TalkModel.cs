using System.ComponentModel.DataAnnotations;

namespace Chatbot.Api.Models
{
    public class TalkModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
