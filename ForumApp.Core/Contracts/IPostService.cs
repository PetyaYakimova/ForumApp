using ForumApp.Core.Models;

namespace ForumApp.Core.Contracts
{
    public interface IPostService
    {
        Task AddAsync(PostModel model);
        Task DeleteAync(int id);
        Task EditAsync(PostModel model);
        Task<IEnumerable<PostModel>> GetAllPostsAsync();
        Task<PostModel?> GetByIdAsync(int id);
    }
}
