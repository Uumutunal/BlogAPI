
using Azure.Core;
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
        private readonly IRepository<PostTag> _postTagRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<FavoritePost> _favoritePostRepository;

        public PostService(IUnitOfWork unitOfWork, IRepository<Post> postRepository, IRepository<PostComment> postCommentRepository, IRepository<PostCategory> postCategoryRepository, IRepository<Category> categoryRepository, IRepository<Comment> commentRepository, IRepository<PostTag> postTagRepository, IRepository<Tag> tagRepository, IRepository<FavoritePost> favoritePostRepository)
        {
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postCategoryRepository = postCategoryRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
            _favoritePostRepository = favoritePostRepository;
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var allCategories = await _categoryRepository.GetAllAsync();
            var allCategoriesMapped = allCategories.Adapt<List<CategoryDto>>();

            return allCategoriesMapped;
        }
        public async Task<List<PostCategoryDto>> GetAllPostCategories()
        {
            var allpostCategories = await _postCategoryRepository.GetAllAsync();
            var allpostCategoriesMapped = allpostCategories.Adapt<List<PostCategoryDto>>();

            return allpostCategoriesMapped;
        }
        public async Task<List<PostCommentDto>> GetAllPostComments()
        {
            var allpostCategories = await _postCommentRepository.GetAllAsync();
            var allpostCategoriesMapped = allpostCategories.Adapt<List<PostCommentDto>>();

            return allpostCategoriesMapped;
        }
        public async Task<List<CommentDto>> GetAllComments()
        {
            var allcomments = await _commentRepository.GetAllAsync();
            var approvedComments = allcomments.Where(x => x.IsApproved);
            var mappedComments = approvedComments.Adapt<List<CommentDto>>();

            return mappedComments;
        }
        public async Task<List<CommentDto>> GetAllUnApprovedComments()
        {
            var allcomments = await _commentRepository.GetAllAsync();
            var approvedComments = allcomments.Where(x => !x.IsApproved);
            var mappedComments = approvedComments.Adapt<List<CommentDto>>();

            return mappedComments;
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
        public async Task DeletePostTag(Guid id)
        {
            await _postTagRepository.Delete(id);
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
        public async Task<bool> UpdateComment(CommentDto comment)
        {

            var commentToUpdate = await _commentRepository.GetByIdAsync(comment.Id);

            commentToUpdate.Content = comment.Content;
            commentToUpdate.ModifiedDate = DateTime.Now;
            await _commentRepository.Update(commentToUpdate);

            return true;
        }

        public async Task CreateFavoritePost(FavoritePostDto favoritePostDto)
        {
            await _favoritePostRepository.AddAsync(favoritePostDto.Adapt<FavoritePost>());
        }
        public async Task<List<FavoritePostDto>> GetAllUserFavorites()
        {
            var allFavoritePosts = await _favoritePostRepository.GetAllAsync();
            //var userFavorites = allFavoritePosts.Where(x => x.UserId == userId).ToList();
            var userFavoritesMapped = allFavoritePosts.Adapt<List<FavoritePostDto>>();
            return userFavoritesMapped;
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
            await _postRepository.Update(postToBeApproved);
        }

        public async Task UpdatePost(AddPostRequest request)
        {
            var postToUpdate = await _postRepository.GetByIdAsync(request.Post.Id);

            postToUpdate.Title = request.Post.Title;
            postToUpdate.Content = request.Post.Content;
            //postToUpdate.Photo = request.Post.Photo;

            if (request.Post.Photo != null)
            {
                postToUpdate.Photo = request.Post.Photo;
            }

            foreach (var tag in request.Tags)
            {
                var newTag = new TagDto()
                {
                    Id = Guid.NewGuid(),
                    Title = tag
                };

                var tagId = await _tagRepository.AddAsync(newTag.Adapt<Tag>());

                var postTag = new PostTagDto()
                {
                    PostId = request.Post.Id,
                    TagId = tagId
                };

                await _postTagRepository.AddAsync(postTag.Adapt<PostTag>());
            }

            await _postRepository.Update(postToUpdate);
        }
        public async Task UpdatePostCategory(PostCategoryDto post)
        {
            var postToBeApproved = post.Adapt<PostCategory>();
            await _postCategoryRepository.Update(postToBeApproved);
        }
        public async Task ApproveComment(Guid id)
        {
            var commentToBeApproved = await _commentRepository.GetByIdAsync(id);
            commentToBeApproved.IsApproved = true;
            await _commentRepository.Update(commentToBeApproved);
        }

        public async Task CreatePost(AddPostRequest request)
        {
            var post = request.Post.Adapt<Post>();
            post.CreatedDate = DateTime.Now;
            post.Id = Guid.NewGuid();

            await _postRepository.AddAsync(post);


            var allCategories = await _categoryRepository.GetAllAsync();
            var categories = allCategories.Where(c => request.CategoryIds.Contains(c.Id)).ToList();

            foreach (var category in categories)
            {
                var postCategory = new PostCategoryDto
                {
                    PostId = post.Id,
                    CategoryId = category.Id,
                    UserId = request.UserId
                };

                var postCategories = postCategory.Adapt<PostCategory>();
                await _postCategoryRepository.AddAsync(postCategories);
            }



            foreach (var tag in request.Tags)
            {
                var newTag = new TagDto()
                {
                    Id = Guid.NewGuid(),
                    Title = tag
                };

                var tagId = await _tagRepository.AddAsync(newTag.Adapt<Tag>());

                var postTag = new PostTagDto()
                {
                    PostId = post.Id,
                    TagId = tagId
                };

                await _postTagRepository.AddAsync(postTag.Adapt<PostTag>());
            }


            //var postComment = new PostComment();

            //postComment.CreatedDate = DateTime.Now;
            //postComment.UserId = request.UserId;
            //postComment.PostId = request.Post.Id;
            //postComment.Id = Guid.NewGuid();
            //postComment.CommentId = Guid.Empty;

            //var mappedPostComment = postComment.Adapt<PostComment>();

            //await _postCommentRepository.AddAsync(postComment);

        }
        public async Task DeleteFavorite(Guid id)
        {
            await _favoritePostRepository.Delete(id);
        }

        public async Task DeletePost(Guid id)
        {
            await _postRepository.Delete(id);
        }

        public async Task<List<PostDto>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var approvedPosts = posts.Where(x => x.IsApproved && !x.IsDraft);
            var mappedPosts = approvedPosts.Adapt<List<PostDto>>();

            return mappedPosts;
        }

        public async Task<List<PostDto>> GetDrafts()
        {
            var posts = await _postRepository.GetAllAsync();
            var draftPosts = posts.Where(x => x.IsDraft);
            var mappedPosts = draftPosts.Adapt<List<PostDto>>();

            return mappedPosts;
        }

        public async Task DeleteComment(Guid id)
        {
            await _commentRepository.Delete(id);
        }

        public async Task<List<PostDto>> GetAllUnApprovedPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var approvedPosts = posts.Where(x => !x.IsApproved && !x.IsDraft);
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
        public async Task<List<TagDto>> GetAllTags()
        {
            var allTags = await _tagRepository.GetAllAsync();
            //Mapper Çalışmıyor
            //var allTagsMapped = allTags.Adapt<List<TagDto>>();
            var allTagsMapped = new List<TagDto>();

            foreach (var item in allTags)
            {
                var newTag = new TagDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    CreatedDate = item.CreatedDate,
                };

                allTagsMapped.Add(newTag);
            }


            return allTagsMapped;
        }
        public async Task<List<PostTagDto>> GetAllPostTags()
        {
            var allPostTags = await _postTagRepository.GetAllAsync();

            var allPostTagsMapped = allPostTags.Adapt<List<PostTagDto>>();

            return allPostTagsMapped;
        }

        public async Task<List<PostCommentDto>> GetAllPostCommentsWithIncludes(params string[] includes)
        {
            var allPosts = await _postCommentRepository.GetAllWithIncludes(includes);

            //var postsMapped = allPosts.ToList().Adapt<List<PostCommentDto>>();

            var postsMapped = allPosts.Select(post => new PostCommentDto
            {
                User = post.User,
                UserId = post.UserId,
                Comment = post.Comment,
                CommentId = post.CommentId,
                Post = post.Post,
                PostId = post.PostId,
                Id = post.Id

            }).ToList();

            return postsMapped;
        }

        public async Task<List<PostCategoryDto>> GetAllPostCategoriesWithIncludes(params string[] includes)
        {
            var allPosts = await _postCategoryRepository.GetAllWithIncludes(includes);

            var postsMapped = allPosts.Adapt<List<PostCategoryDto>>();

            return postsMapped;
        }

        public async Task UpdateDraft(Guid id)
        {
            var postToBeUpdated = await _postRepository.GetByIdAsync(id);
            postToBeUpdated.IsDraft = false;
            await _postRepository.Update(postToBeUpdated);
        }
    }
}
