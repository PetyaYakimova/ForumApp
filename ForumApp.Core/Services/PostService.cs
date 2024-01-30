using ForumApp.Core.Contracts;
using ForumApp.Core.Models;
using ForumApp.Infrastructure.Data;
using ForumApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForumApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly ForumDbContext context;

        private readonly ILogger logger;

        public PostService(ForumDbContext _context, ILogger<PostService> _logger)
        {
            this.context = _context;
            this.logger = _logger;
        }

        public async Task AddAsync(PostModel model)
        {
            var entity = new Post()
            {
                Title = model.Title,
                Content = model.Content
            };

            try
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostService.AddAsync");

                throw new ApplicationException("Operation failed. Please try again");
            }
        }

        public async Task DeleteAync(int id)
        {
            var entity = await GetEntityByIdAsync(id);

            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task EditAsync(PostModel model)
        {
            var entity = await GetEntityByIdAsync(model.Id);

            entity.Title = model.Title;
            entity.Content = model.Content;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            return await context.Posts
                .Select(post => new PostModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PostModel?> GetByIdAsync(int id)
        {
            return await context.Posts
                .Where(p => p.Id == id)
                .Select(p => new PostModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        private async Task<Post> GetEntityByIdAsync(int id)
        {
            var entity = await context
               .FindAsync<Post>(id);

            if (entity == null)
            {
                throw new ApplicationException("Invalid post");
            }

            return entity;
        }
    }
}
