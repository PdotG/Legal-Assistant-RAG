
using AutoMapper;
using backend.Data.Repositories;
using backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using backend.Helpers;
using System.Security.Claims;
using backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/files")]
    [Authorize]
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
            var filesDto = _mapper.Map<IEnumerable<FileDto>>(files);
            return Ok(filesDto);
        }

        [HttpGet("{id_file}")]
        public async Task<ActionResult<FileDto>> GetFileById(int id_file)
        {
            var file = await _repository.GetByIdAsync(id_file);
            if (file == null)
                return NotFound();

            var fileDto = _mapper.Map<FileDto>(file);
            return Ok(fileDto);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto fileUploadDto)
        {
            if (fileUploadDto.File == null || fileUploadDto.File.Length == 0)
                return BadRequest("No se ha subido ningún archivo.");

            if (!fileUploadDto.File.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos PDF.");

            if (fileUploadDto.File.Length > 10 * 1024 * 1024)
                return BadRequest("El archivo excede el tamaño máximo permitido de 10 MB.");

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


                await _pdfHelper.ProcessPdfAsync(filePath, fileEntity.Id);

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

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();
            
            var files = await _repository.GetFilesByUserIdAsync(userId);
            var file = files.FirstOrDefault(f => f.Name == name);

            if (file == null)
                return NotFound(new { message = "File not found or you don't have permission to delete it." });

            if (System.IO.File.Exists(file.FilePath))
            {
                try
                {
                    System.IO.File.Delete(file.FilePath);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error deleting file from disk.", details = ex.Message });
                }
            }

            try
            {
                await _repository.Delete(file);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting file record from database.", details = ex.Message });
            }

            return Ok(new { message = "File deleted successfully." });
        }

    }


}
