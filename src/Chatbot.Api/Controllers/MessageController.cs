using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Chatbot.Api.Services;
using Chatbot.Api.Infrastructure;
using System.Linq;
using Chatbot.Api.Models;

namespace Chatbot.Api.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private TwilioService _smsService;
        private WitService _aiService;
        private HueService _hueService;

        public MessageController(TwilioService smsService, WitService aiService, HueService hueService)
        {
            _smsService = smsService;
            _aiService = aiService;
            _hueService = hueService;
        }
        // GET talk api/message
        [HttpGet()]
        public string Hi(int id)
        {
            return "Hello world!";
        }

        // POST api/message
        [HttpPost]
        public async Task<IActionResult> Post(TwilioSmsRequest request)
        {
            var randomeResponse = new List<string>(new string[] {
             "Hi, you are trying to message a chat bot, are you excited about it?",
             "Hi, how do you like our chat bot?",
             "Hi, i'm so happy to text you back :)"
            });

            var message = await _aiService.ConverseAsync(request.Body, GetContextAsync).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(message))
                message = randomeResponse.Shuffle().First();

            await _smsService.SendSmsAsync(request.From, message).ConfigureAwait(false);
            return Ok();
        }

        private async Task<Dictionary<string, string>> GetContextAsync(WitConverseResponse response)
        {
            if (response == null)
                return null;

            var paymentDueDays = new List<int>(new int[] { 0, 1, 5, 10, 15, 20, 25 });
            var numberOfPayments = new List<int>(new int[] { 18, 20, 3, 9, 1, 0 });
            var paymentAmounts = new List<double>(new double[] { 657.06, 750.20, 987.00, 370.01, 209.77, 557.00 });
            var accountBalance = new List<double>(new double[] { 1050.90, 2006.50, 1500.05, 4900.00, 3402.99 });            

            Dictionary<string, string> context = null;
            if (response.action == "GetPaymentsRemaining")
            {
                context = new Dictionary<string, string> {
                                { "context-number-payments", numberOfPayments.Shuffle().First().ToString() }
                            };
            }
            if (response.action == "GetNextPayment")
            {
                context = new Dictionary<string, string> {
                                { "context-next-payment-amount",  paymentAmounts.Shuffle().First().ToString("C") },
                                { "context-next-payment-date", DateTime.Now.AddDays(paymentDueDays.Shuffle().First()).ToString("D") }
                            };
            }
            if (response.action == "GetAccountBalance")
            {
                context = new Dictionary<string, string> {
                                { "context-accout-balance", accountBalance.Shuffle().First().ToString("C") }
                            };
            }
            if (response.action == "TurnLightOn")
            {            
                var lightIdEntity = response.entities.FirstOrDefault(e => e.Key == "light_id");
                var statusEntity = response.entities.FirstOrDefault(e => e.Key == "on_off");

                if (lightIdEntity.Value != null && lightIdEntity.Value.Count > 0 &&
                    statusEntity.Value != null && statusEntity.Value.Count > 0)
                {
                    var typeValue = lightIdEntity.Value.First().value.ToString();
                    var statusValue = statusEntity.Value.First().value.ToString();

                    var lightRequest = new HueLightStateChangeRequest(typeValue, statusValue);
                    var result = await _hueService.SetLightState(lightRequest).ConfigureAwait(false);
                    if (result)
                    {
                        var light = await _hueService.GetLightStatus(lightRequest.LightId).ConfigureAwait(false);
                        if (light != null)
                        {
                            context = new Dictionary<string, string>
                            {
                                { "context-lightid", typeValue },
                                { "context-lightstatus", light.state.Status }
                            };
                        }
                    }
                }           
            }

            return context;
        }
    }
}
