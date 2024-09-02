using Domain.Core.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Models;
using System.Text.Json;

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
        [HttpGet("AllPostCategories")]
        public async Task<IActionResult> GetAllPostCategories()
        {
            var posts = await _postService.GetAllPostCategories();


            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);
        }

        [HttpGet("GetAllPostComments")]
        public async Task<IActionResult> GetAllPostComments()
        {
            var posts = await _postService.GetAllPostComments();


            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);

        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _postService.GetAllComments();


            if (comments == null || !comments.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(comments);

        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _postService.GetAllCategories();


            if (categories == null || !categories.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(categories);

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

        [HttpPost("ApprovePost")]
        public async Task<IActionResult> ApprovePost([FromBody] Guid id)
        {
            await _postService.ApprovePost(id);

            return Ok();
        }
        [HttpPost("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody] AddPostRequest postToUpdate)
        {

            await _postService.UpdatePost(postToUpdate.Post);

            return Ok();
        }

        [HttpGet("AllUnApprovedPost")]
        public async Task<IActionResult> GetAllUnApprovedPosts()
        {
            var posts = await _postService.GetAllUnApprovedPosts();

            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(posts);
        }

        [HttpPost("AddComment")]
        public async Task AddComment([FromBody] AddCommentRequest request)
        {
           var comment =  await _postService.CreateComment(request.Comment);
            

            var postcomment = new PostCommentDto(){
                PostId = request.PostId,
                UserId = request.UserId,
                CommentId = request.Comment.Id,
            };
            await _postService.CreatePostComment(postcomment);
        }

        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] AddPostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _postService.CreatePost(request);
            return Ok("Post created successfully.");
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
