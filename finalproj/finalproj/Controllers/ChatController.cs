using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using finalproj.BL;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;


namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Chat chat)
        {
           
            if (chat.Insert()==true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("message could not be saved");
            }

        }

        [HttpGet]
        [Route("getChats")]
        public ActionResult<IEnumerable<Chat>> GetFullChat([FromQuery] int user1Id, [FromQuery] int user2Id)
        {
            Chat chat = new Chat();
            return chat.readFullChat(user1Id, user2Id);

        }

        [HttpGet]
        [Route("getLatestChats")]
        public ActionResult<IEnumerable<Chat>> getLatestChats([FromQuery] int userId)
        {
            Chat chat = new Chat();
            return chat.ReadLatestChat(userId);

        }

        [HttpGet]
        [Route("getChatsFromDate")]
        public ActionResult<IEnumerable<Chat>> GetFullChatFromDate([FromQuery] int user1Id, [FromQuery] int user2Id, [FromQuery] DateTime startDate)
        {
            Chat chat = new Chat();
            return chat.readFullChatFromDate(user1Id, user2Id, startDate);
        }
    }
}
