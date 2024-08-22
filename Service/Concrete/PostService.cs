
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

        public async Task ApprovePost(Guid id)
        {
            var postToBeApproved = await _repository.GetByIdAsync(id);
            postToBeApproved.IsApproved = true;
            _repository.Update(postToBeApproved);
        }

        public Task CreatePost(PostDto postDto)
        {

            var mappedPost = postDto.Adapt<Post>();

            return _repository.AddAsync(mappedPost);
        }

        public async Task DeletePost(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _repository.GetAllAsync();
            var approvedPosts = posts.Where(x => x.IsApproved);
            var mappedPosts = approvedPosts.Adapt<List<PostDto>>();

            return mappedPosts;
        }

        public async Task<List<PostDto>> GetAllUnApprovedPosts()
        {
            var posts = await _repository.GetAllAsync();
            var approvedPosts = posts.Where(x => !x.IsApproved);
            var mappedPosts = approvedPosts.Adapt<List<PostDto>>();

            return mappedPosts;
        }

        public Task LikePost(int postId, int userId)
        {
            throw new NotImplementedException();
        }



    }
}
