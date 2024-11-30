using AutoMapper;
using backend.Data.Repositories;
using backend.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAll()
        {
            var clients = await _repository.GetAllAsync();
            var clientsDto = _mapper.Map<IEnumerable<ClientResponseDto>>(clients);
            return Ok(clientsDto);
        }

    }
}