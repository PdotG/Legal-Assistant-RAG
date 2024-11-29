using AutoMapper;
using backend.Data;
using backend.Data.Repositories;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(UserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _repository.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserDto UserDto)
        {
            try
            {
                var user = _mapper.Map<User>(UserDto);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _repository.AddAsync(user);

                return CreatedAtAction(nameof(Create), new { id = user.Id }, new { message = "User created successfully" });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("unique") ?? false)
                {
                    return Conflict(new { message = "The email address is already in use." });
                }

                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            _mapper.Map(userDto, existingUser);
            await _repository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            
            await _repository.Delete(user);
            return NoContent();
        }
    }
}
