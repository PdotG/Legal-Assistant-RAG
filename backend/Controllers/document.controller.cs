
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
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentRepository _repository;
        private readonly IMapper _mapper;

        public DocumentsController(DocumentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentResponseDto>>> GetAll()
        {
            var documents = await _repository.GetAllAsync();
            var documentsDto = _mapper.Map<IEnumerable<DocumentResponseDto>>(documents);
            return Ok(documentsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentResponseDto>> GetById(int id)
        {
            var document = await _repository.GetByIdAsync(id);
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<DocumentResponseDto>(document);
            return Ok(documentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DocumentRequestDto documentRequestDto)
        {
            try
            {
                var document = _mapper.Map<Document>(documentRequestDto);
                await _repository.AddAsync(document);

                return CreatedAtAction(nameof(Create), new { id = document.IdDocument }, new { message = "Document created successfully" });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("unique") ?? false)
                {
                    return Conflict(new { message = "The document is already created." });
                }

                return StatusCode(500, new { message = "An error occurred while saving to the database." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DocumentRequestDto documentRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingDocument = await _repository.GetByIdAsync(id);
            if (existingDocument == null)
                return NotFound();

            _mapper.Map(documentRequestDto, existingDocument);

            existingDocument.UploadedDate = DateTime.UtcNow;
            if (existingDocument.UploadedDate.Kind == DateTimeKind.Unspecified)
            {
                existingDocument.UploadedDate = DateTime.SpecifyKind(existingDocument.UploadedDate, DateTimeKind.Utc);
            }

            await _repository.UpdateAsync(existingDocument);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await _repository.GetByIdAsync(id);
            if (document == null)
                return NotFound();
            
            await _repository.Delete(document);
            return NoContent();
        }
    }
}