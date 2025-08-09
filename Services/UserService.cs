using TestAAIbackend.Dtos;
using TestAAIbackend.Models;
using TestAAIbackend.Repositories;

namespace TestAAIbackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default)
        {
            var users = await _repo.GetAllAsync(ct);
            return users.Select(u => new UserDto { Id = u.Id, Name = u.Name, LastName = u.LastName });
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(id, ct);
            if (user == null) return null;
            return new UserDto { Id = user.Id, Name = user.Name, LastName = user.LastName };
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            var entity = new User { Name = dto.Name, LastName = dto.LastName };
            await _repo.AddAsync(entity, ct);
            return new UserDto { Id = entity.Id, Name = entity.Name, LastName = entity.LastName };
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;
            existing.Name = dto.Name;
            existing.LastName = dto.LastName;
            await _repo.UpdateAsync(existing, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing, ct);
            return true;
        }
    }
}
