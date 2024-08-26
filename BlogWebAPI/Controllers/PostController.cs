using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Models;

namespace BlogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("GetAllPost")]
        public async Task GetAllPost()
        {          
            var posts = await _postService.GetAllPosts();
        }


        [HttpPost("AddPost")]
        public async Task AddPost([FromBody] PostDto postDto)
        {
            await _postService.CreatePost(postDto);
        }

        [HttpPost("Delete")]
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
    }
}
