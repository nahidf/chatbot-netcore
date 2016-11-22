using Microsoft.AspNetCore.Mvc;
using Chatbot.Api.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chatbot.Api.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        TwilioService _smsService;
        WitService _aiService;

        public BotController(TwilioService smsService, WitService aiService)
        {
            _smsService = smsService;
            _aiService = aiService;
        }
        // GET talk
        [HttpGet()]
        public string Hi(int id)
        {
            return "Hello world!";
        }

        // POST api/talk
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TalkModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("model is null or invalid!");

            var message = await _aiService.ConverseAsync(model.Message).ConfigureAwait(false);
            if(!string.IsNullOrWhiteSpace(message))
                await _smsService.SendSmsAsync(model.PhoneNumber, message).ConfigureAwait(false);

            return Ok();
        }
    }
}
