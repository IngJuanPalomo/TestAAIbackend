using TestAAIbackend.Dtos;

namespace TestAAIbackend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
