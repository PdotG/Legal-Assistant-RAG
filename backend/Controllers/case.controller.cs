
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
    public class CasesController : ControllerBase
    {
        private readonly CaseRepository _repository;
        private readonly IMapper _mapper;

        public CasesController(CaseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseResponseDto>>> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();
            var cases = await _repository.GetAllCasesForAUserIdAsync(userId);
            var casesDto = _mapper.Map<IEnumerable<CaseResponseDto>>(cases);
            return Ok(casesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CaseRequestDto>> GetById(int id)
        {
            var caseObj = await _repository.GetByIdAsync(id);
            if (caseObj == null)
                return NotFound();

            var caseDto = _mapper.Map<CaseRequestDto>(caseObj);
            return Ok(caseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CaseRequestDto caseRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var caseObj = _mapper.Map<Case>(caseRequestDto);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized();

                var clientExists = await _repository.ClientExistsAsync(caseRequestDto.ClientId);
                if (!clientExists)
                {
                    return NotFound(new { message = $"Client with ID {caseRequestDto.ClientId} does not exist." });
                }

                caseObj.AssignedUserId = userId;
                await _repository.AddAsync(caseObj);

                return CreatedAtAction(nameof(Create), new { id = caseObj.IdCase }, new { message = "Case created successfully" });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("unique") ?? false)
                {
                    return Conflict(new { message = "The case is already registered." });
                }

                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CaseRequestDto caseRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCase = await _repository.GetByIdAsync(id);
            if (existingCase == null)
                return NotFound();
            
            _mapper.Map(caseRequestDto, existingCase);
            existingCase.UpdatedDate = DateTime.UtcNow;
            if (caseRequestDto.CourtDate.HasValue)
            {
                existingCase.CourtDate = caseRequestDto.CourtDate.Value.ToUniversalTime();
            }

            await _repository.UpdateAsync(existingCase);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var caseObj = await _repository.GetByIdAsync(id);
            if (caseObj == null)
                return NotFound();
            
            await _repository.Delete(caseObj);
            return NoContent();
        }
    }
}