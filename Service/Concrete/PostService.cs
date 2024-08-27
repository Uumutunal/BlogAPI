
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
            category.CreatedDate = DateTime.Now;
            category.Id = Guid.NewGuid();
            await _categoryRepository.AddAsync(category);
        }

        public async Task DeleteCategory(Guid id)
        {
            await _categoryRepository.Delete(id);
        }

        public async Task<bool> UpdateCategory(Guid id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            category.Name = categoryDto.Name;
            category.ModifiedDate = DateTime.Now;

            await _categoryRepository.Update(category);

            return true;
        }

        public async Task<Guid> CreateComment(CommentDto commentDto)
        {
            var comment = commentDto.Adapt<Comment>();
            comment.CreatedDate = DateTime.Now;
            comment.Id = Guid.NewGuid();
            var id = await _commentRepository.AddAsync(comment);
            return id;
        }
        public async Task CreatePostCategory(PostCategoryDto postCategoryDto)
        {
            var post = postCategoryDto.Adapt<PostCategory>();
            await _postCategoryRepository.AddAsync(post);
        }

        public async Task CreatePostComment(PostCommentDto postCommentDto)
        {
            var post = postCommentDto.Adapt<PostComment>();
            post.CreatedDate = DateTime.Now;
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

        public async Task CreatePost(PostDto postDto, List<Guid> categoryIds)
        {
            var post = postDto.Adapt<Post>();
            post.CreatedDate = DateTime.Now;
            post.Id = Guid.NewGuid();

            await _postRepository.AddAsync(post);


            var allCategories = await _categoryRepository.GetAllAsync();
            var categories = allCategories.Where(c => categoryIds.Contains(c.Id)).ToList();

            foreach (var category in categories)
            {
                var postCategory = new PostCategoryDto
                {
                    PostId = post.Id,
                    CategoryId = category.Id
                };
                var postCategories = postCategory.Adapt<PostCategory>();
                await _postCategoryRepository.AddAsync(postCategories);
            }

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

        public async Task DeleteComment(Guid id)
        {
            await _commentRepository.Delete(id);
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

        public async Task<PostDto> GetByIdAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            var mappedPost = post.Adapt<PostDto>();
            return mappedPost;
        }



    }
}
