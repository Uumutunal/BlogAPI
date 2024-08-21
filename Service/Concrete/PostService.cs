using Domain.Core.Repositories;
using Service.Abstract;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class PostService : IPostService
    {

        private readonly IUnitOfWork _unitOfWork;


        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public Task ApprovePost(int postId)
        {
            throw new NotImplementedException();
        }

        public Task CreatePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public Task DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PostDto>> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public Task<List<PostDto>> GetAllUnApprovedPosts()
        {
            throw new NotImplementedException();
        }

        public Task LikePost(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task Register(string email, string password, string confirmPassword)
        {
            //await _register.AssignInput(email, password, confirmPassword);
        }


    }
}
