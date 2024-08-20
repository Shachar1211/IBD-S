using finalproj.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DocumentsController : Controller
    {
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<IEnumerable<Documents>> Get(int userId)
        {
            Documents documents = new Documents();  
            return documents.GetDocumetsByUserId(userId);
        }

        // GET: api/<DocumentsController>/file/{fileId}
        [HttpGet("file/{fileId}")]
        public ActionResult<IEnumerable<Documents>> GetDocumentsByFileId(int fileId)
        {
            Documents documents = new Documents();
            return documents.GetDocumentsByFileId(fileId);
        }


        // POST api/<CalendarEventsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Documents documents)
        {
            Documents Fulldocuments = new Documents();
            Fulldocuments = documents.Insert();
            if (Fulldocuments != null)
            {
                return Ok(Fulldocuments); // Return the created event with full details
            }
            else
            {
                return BadRequest("file could not be added");
            }
        }

        [HttpDelete("{documentId}")]
        public IActionResult Delete(int documentId)
        {
            Documents documents = new Documents { DocumentId = documentId };
            bool result = documents.Delete();
            if (result)
            {
                return Ok("document deleted successfully");
            }
            else
            {
                return NotFound("document not found or not deleted");
            }
        }

    }
}
