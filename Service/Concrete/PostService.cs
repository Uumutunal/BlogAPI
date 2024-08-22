using Domain.Core.Repositories;
using Domain.Entities;
using Mapster;
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
        private readonly IRepository<Post> _repository;

        public PostService(IUnitOfWork unitOfWork, IRepository<Post> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public Task ApprovePost(int postId)
        {
            throw new NotImplementedException();
        }

        public Task CreatePost(PostDto postDto)
        {

            var mappedPost = postDto.Adapt<Post>();

            return _repository.AddAsync(mappedPost);
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



    }
}
