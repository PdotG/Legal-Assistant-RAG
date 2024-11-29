
using AutoMapper;
using backend.Data.Repositories;
using backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using backend.Helpers;
using System.Security.Claims;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileRepository _repository;
        private readonly IMapper _mapper;
        private readonly PdfHelper _pdfHelper;

        public FilesController(FileRepository repository, IMapper mapper, PdfHelper pdfHelper)
        {
            _repository = repository;
            _mapper = mapper;
            _pdfHelper = pdfHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetAll()
        {
            var files = await _repository.GetAllAsync();
            var filesDto = _mapper.Map<IEnumerable<UserDto>>(files);
            return Ok(filesDto);
        }

        [HttpGet("{id_file}")]
        public async Task<ActionResult<UserDto>> GetFileById(int id_file)
        {
            var file = await _repository.GetByIdAsync(id_file);
            if (file == null)
                return NotFound();

            var fileDto = _mapper.Map<UserDto>(file);
            return Ok(fileDto);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileUploadDto)
        {
            // Validar que se haya recibido un archivo
            if (fileUploadDto.File == null || fileUploadDto.File.Length == 0)
                return BadRequest("No se ha subido ningún archivo.");

            // Validaciones adicionales
            if (!fileUploadDto.File.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos PDF.");

            if (fileUploadDto.File.Length > 10 * 1024 * 1024) // 10 MB
                return BadRequest("El archivo excede el tamaño máximo permitido de 10 MB.");

            // Obtener el UserId del usuario autenticado
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();

            try
            {
                var filePath = _pdfHelper.SaveFile(fileUploadDto.File);

                var fileEntity = new Models.File
                {
                    Name = fileUploadDto.Name,
                    ScrapedAt = fileUploadDto.ScrapedAt,
                    FilePath = filePath,
                    UserId = userId,
                };

                await _repository.AddAsync(fileEntity);

                return Ok(new 
                { 
                    fileEntity.Id, 
                    fileEntity.Name, 
                    fileEntity.FilePath, 
                    fileEntity.ScrapedAt 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al subir el archivo.");
            }
        }
    }


}
