using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BlogWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AdminController(UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration, IServiceProvider serviceProvider, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        [HttpPost("AddRole")]
        public async Task AddRole(string[] roles)
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityResult roleResult;

            foreach (var roleName in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        //[HttpDelete("roles/{roleName}")]
        //public async Task<IActionResult> DeleteRole(string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);

        //    if (role == null)
        //        return NotFound("Role not found.");

        //    var result = await _roleManager.DeleteAsync(role);

        //    if (result.Succeeded)
        //        return Ok("Role deleted successfully.");
        //    else
        //        return BadRequest(result.Errors);
        //}

        //[HttpPost("assign-role")]
        //public async Task<IActionResult> AssignRoleToUser([FromBody] IdentityUserRole<string> model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    if (user == null)
        //        return NotFound("User not found.");

        //    var result = await _userManager.AddToRoleAsync(user, model.RoleId);

        //    if (result.Succeeded)
        //        return Ok("Role assigned successfully.");
        //    else
        //        return BadRequest(result.Errors);
        //}
    }
}
