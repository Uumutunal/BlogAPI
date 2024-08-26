
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
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly IRepository<PostCategory> _postCategoryRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Comment> _commentRepository;

        public PostService(IUnitOfWork unitOfWork, IRepository<Post> postRepository, IRepository<PostComment> postCommentRepository, IRepository<PostCategory> postCategoryRepository, IRepository<Category> categoryRepository, IRepository<Comment> commentRepository)
        {
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postCategoryRepository = postCategoryRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
        }

        public async Task CreateCategory(CategoryDto categoryDto)
        {
            var category = categoryDto.Adapt<Category>();
            await _categoryRepository.AddAsync(category);
        }
        public async Task CreateComment(CommentDto commentDto)
        {
            var comment = commentDto.Adapt<Comment>();
            await _commentRepository.AddAsync(comment);
        }
        public async Task CreatePostCategory(PostCategoryDto postCategoryDto)
        {
            var post = postCategoryDto.Adapt<PostCategory>();
            await _postCategoryRepository.AddAsync(post);
        }

        public async Task CreatePostComment(PostCommentDto postCommentDto)
        {
            var post = postCommentDto.Adapt<PostComment>();
            await _postCommentRepository.AddAsync(post);
        }

        public async Task ApprovePost(Guid id)
        {
            var postToBeApproved = await _postRepository.GetByIdAsync(id);
            postToBeApproved.IsApproved = true;
            _postRepository.Update(postToBeApproved);
        }

        public async Task ApproveComment(Guid id)
        {
            var commentToBeApproved = await _commentRepository.GetByIdAsync(id);
            commentToBeApproved.IsApproved = true;
            _commentRepository.Update(commentToBeApproved);
        }

        public Task CreatePost(PostDto postDto)
        {

            var mappedPost = postDto.Adapt<Post>();

            return _postRepository.AddAsync(mappedPost);
        }

        public async Task DeletePost(Guid id)
        {
            await _postRepository.Delete(id);
        }

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var approvedPosts = posts.Where(x => x.IsApproved);
            var mappedPosts = approvedPosts.Adapt<List<PostDto>>();

            return mappedPosts;
        }

        public async Task<List<PostDto>> GetAllUnApprovedPosts()
        {
            var posts = await _postRepository.GetAllAsync();
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
