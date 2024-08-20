using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumAnswersController : ControllerBase
    {
        [HttpGet("{questionId}")]
        public ActionResult<IEnumerable<ForumAnswer>> GetByQuestion(int questionId)
        {
            return ForumAnswer.ReadByQuestion(questionId);
        }

        [HttpGet("answer/{answerId}")]
        public ActionResult<ForumAnswer> Get(int answerId)
        {
            var answer = ForumAnswer.ReadOne(answerId);
            if (answer != null)
            {
                return Ok(answer);
            }
            else
            {
                return NotFound("Answer not found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ForumAnswer answer)
        {
            if (answer.Insert())
            {
                return Ok(answer);
            }
            else
            {
                return BadRequest("Answer could not be added");
            }
        }

        [HttpPut("{answerId}")]
        public IActionResult Put(int answerId, [FromBody] ForumAnswer answer)
        {
            answer.AnswerId = answerId;
            if (answer.Update() > 0)
            {
                return Ok(answer);
            }
            else
            {
                return BadRequest("Answer not updated");
            }
        }

        [HttpDelete("{answerId}")]
        public IActionResult Delete(int answerId)
        {
            if (ForumAnswer.Delete(answerId) > 0)
            {
                return Ok("Answer deleted successfully");
            }
            else
            {
                return NotFound("Answer not found or not deleted");
            }
        }
    }
}
