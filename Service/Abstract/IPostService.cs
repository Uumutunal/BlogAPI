using Domain.Entities;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IPostService
    {
        Task CreatePost(AddPostRequest request);
        Task<List<PostDto>> GetAllPosts();
        Task<List<PostDto>> GetAllUnApprovedPosts();
        Task<List<CategoryDto>> GetAllCategories();
        Task ApprovePost(Guid id);
        Task DeletePost(Guid id);
        Task UpdatePost(AddPostRequest request);
        Task UpdatePostCategory(PostCategoryDto post);
        Task LikePost(int postId, int userId);
        Task<Guid> CreateComment(CommentDto commentDto);
        Task ApproveComment(Guid id);
        Task<List<CommentDto>> GetAllUnApprovedComments();
        Task DeleteComment(Guid id);
        Task<bool> UpdateComment(CommentDto comment);
        Task DeletePostTag(Guid id);
        Task<PostDto> GetByIdAsync(Guid id);
        Task<List<PostTagDto>> GetAllPostTags();
        Task<List<TagDto>> GetAllTags();
        Task CreatePostComment(PostCommentDto postCommentDto);
        Task CreateCategory(CategoryDto categoryDto);
        Task DeleteCategory(Guid id);
        Task<bool> UpdateCategory(Guid id, CategoryDto categoryDto);
        Task<List<PostCategoryDto>> GetAllPostCategories();
        Task<List<PostCommentDto>> GetAllPostCommentsWithIncludes(params string[] includes);
        Task<List<PostCategoryDto>> GetAllPostCategoriesWithIncludes(params string[] includes);
        Task<List<PostCommentDto>> GetAllPostComments();
        Task<List<CommentDto>> GetAllComments();

    }
}
