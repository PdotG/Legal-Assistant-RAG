using System.Security.Claims;
using AutoMapper;
using backend.Data.Repositories;
using backend.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly ClientRepository _repository;
        private readonly IMapper _mapper;

        public ClientsController(ClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAllClientsByIdUser(int idUser)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();

            if (currentUserId != idUser)
                return Forbid();

            var clients = await _repository.GetClientsByUserIdAsync(idUser);
            var clientsDto = _mapper.Map<IEnumerable<ClientResponseDto>>(clients);
            return Ok(clientsDto);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAllClientsByCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();

            var clients = await _repository.GetClientsByUserIdAsync(currentUserId);
            var clientsDto = _mapper.Map<IEnumerable<ClientResponseDto>>(clients);
            return Ok(clientsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseDto>> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();

            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            if (currentUserId != client.IdUser)
                return Forbid();

            var clientDto = _mapper.Map<ClientResponseDto>(client);
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientRequestDto clientRequestDto)
        {
            try
            {
                var client = _mapper.Map<Client>(clientRequestDto);
                await _repository.AddAsync(client);

                return CreatedAtAction(nameof(Create), new { id = client.IdClient }, new { message = "Client created successfully" });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("unique") ?? false)
                {
                    return Conflict(new { message = "The client is already registered." });
                }

                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClientRequestDto clientRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingClient = await _repository.GetByIdAsync(id);
            if (existingClient == null)
                return NotFound();

            _mapper.Map(clientRequestDto, existingClient);
            await _repository.UpdateAsync(existingClient);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            await _repository.Delete(client);
            return NoContent();
        }

    }
}