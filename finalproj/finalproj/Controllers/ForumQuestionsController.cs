using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumQuestionsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ForumQuestion>> Get()
        {
            return ForumQuestion.ReadAll();
        }

        [HttpGet("{questionId}")]
        public ActionResult<ForumQuestion> Get(int questionId)
        {
            var question = ForumQuestion.ReadOne(questionId);
            if (question != null)
            {
                return Ok(question);
            }
            else
            {
                return NotFound("Question not found");
            }
        }

        [HttpGet("topic/{topic}")]
        public ActionResult<IEnumerable<ForumQuestion>> GetByTopic(string topic)
        {
            return ForumQuestion.ReadByTopic(topic);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ForumQuestion question)
        {
            if (question.Insert())
            {
                return Ok(question);
            }
            else
            {
                return BadRequest("Question could not be added");
            }
        }

        [HttpPut("{questionId}")]
        public IActionResult Put(int questionId, [FromBody] ForumQuestion question)
        {
            question.QuestionId = questionId;
            if (question.Update() > 0)
            {
                return Ok(question);
            }
            else
            {
                return BadRequest("Question not updated");
            }
        }

        [HttpDelete("{questionId}")]
        public IActionResult Delete(int questionId)
        {
            if (ForumQuestion.Delete(questionId) > 0)
            {
                return Ok("Question deleted successfully");
            }
            else
            {
                return NotFound("Question not found or not deleted");
            }
        }
    }
}
