using TestAAIbackend.Models;

namespace TestAAIbackend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(User user, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}
