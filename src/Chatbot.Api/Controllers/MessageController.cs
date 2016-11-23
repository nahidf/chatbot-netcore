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
        TwilioService _smsService;
        WitService _aiService;

        public MessageController(TwilioService smsService, WitService aiService)
        {
            _smsService = smsService;
            _aiService = aiService;
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

            var message = await _aiService.ConverseAsync(request.Body, GetContext).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(message))
                message = randomeResponse.Shuffle().First();

            await _smsService.SendSmsAsync(request.From, message).ConfigureAwait(false);
            return Ok();
        }


        private Dictionary<string, string> GetContext(string actionName)
        {
            var paymentDueDays = new List<int>(new int[] { 0, 1, 5, 10, 15, 20, 25 });
            var numberOfPayments = new List<int>(new int[] { 18, 20, 3, 9, 1, 0 });
            var paymentAmounts = new List<double>(new double[] { 657.06, 750.20, 987.00, 370.01, 209.77, 557.00 });
            var accountBalance = new List<double>(new double[] { 1050.90, 2006.50, 1500.05, 4900.00, 3402.99 });

            Dictionary<string, string> context = null;
            if (actionName == "GetPaymentsRemaining")
            {
                context = new Dictionary<string, string> {
                                { "context-number-payments", numberOfPayments.Shuffle().First().ToString() }
                            };
            }
            if (actionName == "GetNextPayment")
            {
                context = new Dictionary<string, string> {
                                { "context-next-payment-amount",  paymentAmounts.Shuffle().First().ToString("C") },
                                { "context-next-payment-date", DateTime.Now.AddDays(paymentDueDays.Shuffle().First()).ToString("D") }
                            };
            }
            if (actionName == "GetAccountBalance")
            {
                context = new Dictionary<string, string> {
                                { "context-accout-balance", accountBalance.Shuffle().First().ToString("C") }
                            };
            }
            return context;
        }
    }
}
