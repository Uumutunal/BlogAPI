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
        Task CreatePost(PostDto postDto);
        Task<List<PostDto>> GetAllPosts();
        Task<List<PostDto>> GetAllUnApprovedPosts();
        Task ApprovePost(int postId);
        Task DeletePost(int postId);
        Task LikePost(int postId, int userId);
        Task Register(string email, string password, string confirmPassword);
    }
}
