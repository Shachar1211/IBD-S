using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using finalproj.BL;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            User user = new User();
            return user.Read();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            // ניסיון להכניס את המשתמש למסד הנתונים
            if (user.Insert()) 
            {
                User FullUser = new User();
                FullUser = user.ReadOne(user.Email);
                return Ok(FullUser);
            }
            else
            {
                return BadRequest("User could not be added. Email or username might not be unique.");
            }
        }

      

        //כניסת משתמש קיים
        [HttpPost("LogIn")]
        public IActionResult LogIn([FromQuery] string email, [FromQuery] string password)
        {
            User user = new User { Email = email, Password = password };
            User loggedInUser = user.LogIn();

            if (loggedInUser != null)
            {
                return Ok(loggedInUser);
            }
            else
            {
                return NotFound("User not found or invalid credentials");
            }
        }

        // עדכון פרטי משתמש

        // PUT api/<UsersController>/5
        [HttpPut("{email}")]
        public IActionResult Put([FromBody] User user)
        {
            if (user.Update() == 1)
            {
                User FullUser = new User();
                FullUser = user.ReadOne(user.Email);
                return Ok(FullUser);
            }
            else
            {
                return BadRequest("User not updated");
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{email}")]
        public IActionResult Delete([FromBody] User user)
        {
            if (user.Delete() == true) 
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return NotFound("User not found or not deleted");
            }
        }

  

        [HttpPost("{userId}/AddFriend/{friendId}")]
        public IActionResult AddFriend(int userId, int friendId)
        {
            User user = new User { Id = userId };
            if (user.AddFriend(friendId))
            {
                return Ok("Friend added successfully.");
            }
            else
            {
                return BadRequest("Friend could not be added.");
            }
        }

        [HttpGet("{userId}/Friends")]
        public IActionResult GetFriends(int userId)
        {
            User user = new User { Id = userId };
            var friends = user.GetFriends();
            return Ok(friends);
        }
    }
}


