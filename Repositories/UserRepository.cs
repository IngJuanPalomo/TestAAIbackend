using Microsoft.EntityFrameworkCore;
using TestAAIbackend.Data;
using TestAAIbackend.Models;

namespace TestAAIbackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Users
                            .AsNoTracking()
                            .OrderBy(u => u.Id)
                            .ToListAsync(ct);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Users.FindAsync(new object[] { id }, ct);
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _db.Users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _db.Users.AsNoTracking().AnyAsync(u => u.Id == id, ct);
        }
    }
}
