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
        Task CreatePost(PostDto postDto, List<Guid> categoryIds);
        Task<List<PostDto>> GetAllPosts();
        Task<List<PostDto>> GetAllUnApprovedPosts();
        Task ApprovePost(Guid id);
        Task DeletePost(Guid id);
        Task LikePost(int postId, int userId);
        Task<Guid> CreateComment(CommentDto commentDto);
        Task ApproveComment(Guid id);
        Task DeleteComment(Guid id);
        Task<PostDto> GetByIdAsync(Guid id);
        Task CreatePostComment(PostCommentDto postCommentDto);
        Task CreateCategory(CategoryDto categoryDto);
        Task DeleteCategory(Guid id);
        Task<bool> UpdateCategory(Guid id, CategoryDto categoryDto);

    }
}
