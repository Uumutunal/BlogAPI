using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BlogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public AdminController(IPostService postService,IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpGet("ApprovePost")]
        public async Task<IActionResult> ApprovePosts(Guid postId)
        {
            try
            {
                await _postService.ApprovePost(postId);
                return Ok(new { result = "Post confirmation completed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ApproveComment")]
        public async Task<IActionResult> ApproveComment(Guid commentId)
        {
            try
            {
                await _postService.ApproveComment(commentId);
                return Ok(new { result = "Comment confirmation completed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("DeletePost")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _postService.DeletePost(id);
                return Ok(new { result = "Post deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("Assign-Role")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] string userEmail, [FromQuery] string roleName)
        {
            await _userService.AssignRoleToUser(userEmail, roleName);
            return Ok($"{roleName} role assigned to {userEmail}.");
        }

        [HttpPost("Add-Role")]
        public async Task<IActionResult> AddRoles([FromBody] string[] roles)
        {
            await _userService.AddRole(roles);
            return Ok("Roles added successfully.");
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateUserRole([FromQuery] string userId, [FromQuery] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be null or empty.");
            }

            var result = await _userService.UpdateUserRoleAsync(userId, roleName);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user role.");
            }

            return Ok(); 
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrEmpty(categoryDto.Name))
            {
                return BadRequest("Category name is required.");
            }

            await _postService.CreateCategory(categoryDto);
            return Ok("Category created successfully.");
        }

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {

            try
            {
                await _postService.DeleteCategory(id);
                return Ok(new { result = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrEmpty(categoryDto.Name))
            {
                return BadRequest("Category name is required.");
            }

            var result = await _postService.UpdateCategory(id, categoryDto);
            if (!result)
            {
                return NotFound("Category not found.");
            }

            return Ok("Category updated successfully.");
        }

    }
}
