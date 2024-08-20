using finalproj.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Mail mail)
        {

            if (mail.Insert() == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("message could not be saved");
            }

        }
        [HttpGet]
        [Route("getMailForUser")]
        public ActionResult<IEnumerable<Mail>> getMailForUser([FromQuery] int userId)
        {
            Mail mail = new Mail();
            return mail.ReadMail(userId);

        }


        [HttpDelete("Question/{forumQuestionId}")]
        public IActionResult DeleteByQuestion(int forumQuestionId)
        {
            Mail mail = new Mail();
            if (mail.DeleteByQuestion(forumQuestionId) == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("delete/{calanderID}")]
        public IActionResult DeleteByCalander(int calanderID)
        {
            Mail mail = new Mail();
            if (mail.DeleteByCalender(calanderID))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }
}
