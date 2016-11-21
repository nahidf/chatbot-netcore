using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot.Api.Models
{
    public class TalkModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
