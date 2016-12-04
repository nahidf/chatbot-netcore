using System;
using System.Collections;
using System.Collections.Generic;
using Chatbot.Api.Models;

namespace Chatbot.Api.Models
{
    public class HueSettings
    {
        public string BaseUri { get; set; }

        public string Username { get; set; }

        public List<HueLight> Lights { get; set; }
    }
}