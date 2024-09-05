using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract;
using Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace BlogWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("AllUser")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUserById([FromQuery] string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var loggedInUser = await _userService.Login(userDto.Email, userDto.Password);
            
            if (loggedInUser != null)
            {
                var token = await _userService.GenerateJwtToken(loggedInUser);
                return Ok(new { Token = token, UserId = loggedInUser.Id });
            }

            return Unauthorized("Invalid login attempt.");
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userDto)
        {
            var user = await _userService.Register(userDto);
            if (user == null)
            {
                return BadRequest("User registration failed.");
            }
            return Ok(user);
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<string>> GetAllRoles()
        {
            var roles = await _userService.GetAllRoles();

            return Ok(roles);
        }


        [HttpGet("GetUserRoleById")]
        public async Task<ActionResult<string>> GetUserRoleById([FromQuery] string id)
        {
            var roles = await _userService.GetUserRoleById(id);

            if(roles != null)
            {
                return Ok(roles);
            }

            return NotFound();
        }

        [HttpPost("DeleteAccount")]
        public async Task<ActionResult<string>> DeleteAccount([FromQuery] string id)
        {
            var result = await _userService.DeleteUser(id);

            if (result != null)
            {
                return Ok(result);

            }


            return NotFound();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {

            var user = await _userService.GetUserById(userDto.Id);

            var loggedInUser = await _userService.Login(userDto.Email, userDto.Password);

            if (loggedInUser == null)
            {
                return Unauthorized();

            }

            if (userDto.ConfirmPassword != "")
            {
                user.Password = userDto.ConfirmPassword;
            }
            else
            {
                user.Password = userDto.Password;
            }

            user.Firstname = userDto.Firstname;
            user.Lastname = userDto.Lastname;
            user.Email = userDto.Email;

            if(userDto.Photo != null)
            {
                user.Photo = userDto.Photo;
            }


            var updatedUser = await _userService.UpdateUser(user);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }

}

