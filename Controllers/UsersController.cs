using Microsoft.AspNetCore.Mvc;
using TestAAIbackend.Dtos;
using TestAAIbackend.Services;

namespace TestAAIbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var users = await _service.GetAllAsync(ct);
            return Ok(users);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var user = await _service.GetByIdAsync(id, ct);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
        {
            var created = await _service.CreateAsync(dto, ct);
            return CreatedAtRoute("GetUserById", new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, dto, ct);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
