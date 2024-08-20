using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RightsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RightsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("QueryChatGPT")]

        public async Task<ActionResult<string>> QueryChatGPT([FromBody] UserParams userParams)
        {
            try
            {
                ChatGPTResponse chatGPTResponse = new ChatGPTResponse(_configuration, userParams);
                string result = await chatGPTResponse.CreateGPT(); // פעולה שעלולה לזרוק שגיאה
                return Ok(result); // אם הפעולה הצליחה, החזר את התוצאה
            }
            catch (Exception ex) // תפוס כל סוג של Exception
            {
                // תגובת שגיאה למשתמש עם הודעת שגיאה מתאימה
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            }

        }
}
