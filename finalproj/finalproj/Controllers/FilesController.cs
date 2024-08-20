using finalproj.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;


namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<IEnumerable<Files>> Get(int userId)
        {
            Files file = new Files(); 
            return file.GetFilesByUserId(userId);
        }


        // POST api/<CalendarEventsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Files file)
        {
            Files fullfile = new Files();
            fullfile = file.Insert();
            if (fullfile != null)
            {
                return Ok(fullfile); // Return the created event with full details
            }
            else
            {
                return BadRequest("file could not be added");
            }
        }


        [HttpDelete("{FilesId}")]
        public IActionResult Delete(int FilesId)
        {
            Files file = new Files { FilesId = FilesId };
            bool result = file.Delete();

            if (result)
            {
                return Ok("File deleted successfully");
            }
            else
            {
                return NotFound("File not found or not deleted");
            }
        }


        // PUT api/<CalendarEventsController>/5
        [HttpPut("{FilesId}")]
        public IActionResult Put(Files file)
        {
            Files fullfile = new Files();
            fullfile = file.Update();
            if (fullfile != null)
            {
                return Ok(fullfile); // Return the updated event with full details
            }
            else
            {
                return BadRequest("Event not updated");
            }
        }
    }
}
