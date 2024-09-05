using Azure.Core;
using Domain.Core.Repositories;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Models;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text.Json;

namespace BlogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
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

        [HttpGet("GetPost")]
        public async Task<IActionResult> GetPost([FromQuery] Guid id)
        {
            var resultCommentAll = await _postService.GetAllPostCommentsWithIncludes(new[] {"User", "Comment", "Post"});
            var resultComment = resultCommentAll.Where(x => x.PostId == id).ToList();

            var resultCategoryAll = await _postService.GetAllPostCategoriesWithIncludes(new[] {"Category"});
            var resultCategory = resultCategoryAll.FirstOrDefault(x => x.PostId == id);


            var response = new List<PostResponse>();

            foreach (var item in resultComment)
            {

                var rep = new PostResponse()
                {
                    Comment = item.Comment.Adapt<CommentDto>(),
                    User = item.User.Adapt<UserDto>(),
                    Post = item.Post.Adapt<PostDto>(),
                    Category = resultCategory == null ? null : resultCategory.Category.Adapt<CategoryDto>(),
                };

                response.Add(rep);
            }



            return Ok(response);

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

        [HttpGet("GetAllUnApprovedComments")]
        public async Task<IActionResult> GetAllUnApprovedComments()
        {
            var comments = await _postService.GetAllUnApprovedComments();


            if (comments == null || !comments.Any())
            {
                return Ok(new List<CommentDto>() { });
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
        [HttpGet("GetAllPostTags")]
        public async Task<IActionResult> GetAllPostTags()
        {
            var allPostTags = await _postService.GetAllPostTags();
            var allTags = await _postService.GetAllTags();


            foreach (var tag in allPostTags)
            {
                tag.Tag = allTags.FirstOrDefault(x => x.Id == tag.TagId);
            }

            return Ok(allPostTags);
        }



        [HttpPost("ApprovePost")]
        public async Task<IActionResult> ApprovePost([FromBody] Guid id)
        {
            await _postService.ApprovePost(id);

            return Ok();
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
        [HttpPost("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody] AddPostRequest postToUpdate)
        {

            foreach (var item in postToUpdate.PostTagIds)
            {
                await _postService.DeletePostTag(item);
            }

            await _postService.UpdatePost(postToUpdate);

            return Ok();
        }

        [HttpPost("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDto comment)
        {

            await _postService.UpdateComment(comment);

            return Ok();
        }

        [HttpGet("AllUnApprovedPost")]
        public async Task<IActionResult> GetAllUnApprovedPosts()
        {
            var posts = await _postService.GetAllUnApprovedPosts();

            //if (posts == null || !posts.Any())
            //{
            //    return NotFound("No posts found.");
            //}

            return Ok(posts);
        }

        [HttpPost("AddComment")]
        public async Task AddComment([FromBody] AddCommentRequest request)
        {
            var commentId = await _postService.CreateComment(request.Comment);

            var postcomment = new PostCommentDto()
            {
                PostId = request.PostId,
                UserId = request.UserId,
                CommentId = commentId,
            };
            await _postService.CreatePostComment(postcomment);
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

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromBody] Guid id)
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

        [HttpPost("ApproveComment")]
        public async Task<IActionResult> ApproveComment([FromBody] Guid id)
        {
            await _postService.ApproveComment(id);

            return Ok();
        }

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromBody] Guid id)
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
