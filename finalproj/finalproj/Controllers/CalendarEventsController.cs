using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;
using System.Threading.Tasks;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        // GET: api/<CalendarEventsController>/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<CalendarEvent>> Get(int userId)
        {
            CalendarEvent calendarEvent = new CalendarEvent();
            return calendarEvent.Read(userId);
        }

        // GET api/<CalendarEventsController>/event/{eventId}
        [HttpGet("event/{eventId}")]
        public IActionResult GetByEventId(int eventId)
        {
            CalendarEvent calendarEvent = new CalendarEvent();
            CalendarEvent result = calendarEvent.ReadOne(eventId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Event not found");
            }
        }

        // POST api/<CalendarEventsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CalendarEvent calendarEvent)
        {
            CalendarEvent fullEvent = new CalendarEvent();
            fullEvent = calendarEvent.Insert();
            if (fullEvent != null)
            {
                return Ok(fullEvent); // Return the created event with full details
            }
            else
            {
                return BadRequest("Event could not be added");
            }
        }

        // PUT api/<CalendarEventsController>/5
        [HttpPut("{eventId}")]
        public IActionResult Put(int eventId, [FromBody] CalendarEvent calendarEvent)
        {
            calendarEvent.EventId = eventId;
            int result = calendarEvent.Update();

            if (result > 0)
            {
                CalendarEvent updatedEvent = calendarEvent.ReadOne(eventId);
                return Ok(updatedEvent); // Return the updated event with full details
            }
            else
            {
                return BadRequest("Event not updated");
            }
        }

        // DELETE api/<CalendarEventsController>/5
        [HttpDelete("{eventId}")]
        public IActionResult Delete(int eventId)
        {
            CalendarEvent calendarEvent = new CalendarEvent { EventId = eventId };
            bool result = calendarEvent.Delete();

            if (result)
            {
                return Ok("Event deleted successfully");
            }
            else
            {
                return NotFound("Event not found or not deleted");
            }
        }

        [HttpDelete("parent/{ParentEvent}")]
        public IActionResult DeleteByParent(int ParentEvent)
        {
            CalendarEvent calendarEvent = new CalendarEvent { ParentEvent = ParentEvent };
            bool result = calendarEvent.DeleteByParentEvent();

            if (result)
            {
                return Ok("Event deleted successfully");
            }
            else
            {
                return NotFound("Event not found or not deleted");
            }
        }
    }
}
