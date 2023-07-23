using expensetrackertry.DTO;
using expensetrackertry.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace expensetrackertry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ExpenseTrackerContext DBContext;

        public UserController(ExpenseTrackerContext dBContext)
        {
            DBContext = dBContext;
        }
        [HttpGet("LogIn")]
        public async Task<ActionResult<UserDTO>> LogIn(string username, string password)
        {
            UserDTO user = await DBContext.Users.Select(a => new UserDTO
            {
                Id = a.Id,
                Username = a.Username,
                Password = a.Password,
                CreatedAt = a.CreatedAt,
                UpdatedAt = DateTime.Now,
            }).FirstOrDefaultAsync(a => a.Username == username && a.Password == password);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(UserDTO userDTO)
        {
            // Check if the username already exists
            bool usernameExists = await DBContext.Users.AnyAsync(u => u.Username == userDTO.Username);
            if (usernameExists)
            {
                return BadRequest("Username already exists.");
            }

            // Create a new User entity based on the UserDTO
            User user = new User
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            // Add the new user to the database
            DBContext.Users.Add(user);
            await DBContext.SaveChangesAsync();

            // Return the created UserDTO
            return CreatedAtAction(nameof(LogIn), new { username = user.Username, password = user.Password }, userDTO);
        }


    }
}
