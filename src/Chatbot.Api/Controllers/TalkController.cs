using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chatbot.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chatbot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TalkController : Controller
    {
        // GET talk
        [HttpGet()]
        public string Verify(int id)
        {
            return "talk is verified!";
        }

        // POST api/talk
        [HttpPost]
        public IActionResult Post([FromBody]TalkModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("model is null or invalid!");

            //1- Get user by phone number 
            //2- Call www.wit.ai to find the method to call 
            //3- Call mysite apis to retrieve data 
            //4- Return answer  

            return Ok();
        }
    }
}
