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
        private readonly IUserService _userService;

        public PostController(IPostService postService,IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpGet("AllPost")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();

            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);
        }

        [HttpPost("AddComment")]
        public async Task AddComment([FromBody] CommentDto commentDto, [FromQuery] string userId, [FromQuery] Guid postId)
        {
           var commentId =  await _postService.CreateComment(commentDto);
            

            var postcomment = new PostCommentDto(){
                PostId = postId,
                UserId = userId,
                CommentId = commentId
            };
            await _postService.CreatePostComment(postcomment);
        }

        [HttpPost("AddPost")]
        public async Task AddPost([FromBody] PostDto postDto)
        {
            await _postService.CreatePost(postDto);
        }

        [HttpPost("DeletePost")]
        public async Task<IActionResult> DeletePost(Guid id)
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

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _postService.DeleteComment(id);
                return Ok(new { result = "Comment deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
