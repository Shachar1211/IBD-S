using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;
using System.Threading.Tasks;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        // GET api/<AlertsController>/event/5
        [HttpGet("event/{eventId}")]
        public ActionResult<IEnumerable<Alert>> GetAlertsByEvent(int eventId)
        {
            Alert alert = new Alert();
            var alerts = alert.ReadByEvent(eventId);

            if (alerts == null || alerts.Count == 0)
            {
                return NotFound("No alerts found for the given event");
            }

            return Ok(alerts);
        }

        // GET api/<AlertsController>/5
        [HttpGet("{alertId}")]
        public IActionResult Get(int alertId)
        {
            Alert alert = new Alert();
            Alert result = alert.ReadOne(alertId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Alert not found");
            }
        }

        // POST api/<AlertsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Alert alert, [FromQuery] int userId)
        {
            Alert fullAlert = new Alert();
            fullAlert = alert.Insert(userId);

            if (fullAlert != null)
            {
                Alert newAlert = alert.ReadOne(alert.AlertId);
                return Ok(newAlert); // Return the created alert with full details
            }
            else
            {
                return BadRequest("Alert could not be added");
            }
        }

        // PUT api/<AlertsController>/5
        [HttpPut("{alertId}")]
        public IActionResult Put(int alertId, [FromBody] Alert alert)
        {
            alert.AlertId = alertId;
            int result = alert.Update();

            if (result > 0)
            {
                Alert updatedAlert = alert.ReadOne(alertId);
                return Ok(updatedAlert); // Return the updated alert with full details
            }
            else
            {
                return BadRequest("Alert not updated");
            }
        }

        // DELETE api/<AlertsController>/alert/5
        [HttpDelete("alert/{alertId}")]
        public IActionResult DeleteAlert(int alertId)
        {
            Alert alert = new Alert { AlertId = alertId };
            bool result = alert.Delete();

            if (result)
            {
                return Ok("Alert deleted successfully");
            }
            else
            {
                return NotFound("Alert not found or not deleted");
            }
        }


        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Alert>> GetAlertByUserID(int userId)
        {
            
            Alert alert = new Alert();
        
            return alert.GetAlertsByUserId(userId);
        }
    }
}
