using AutoMapper;
using backend.Dtos;
using backend.Models;
using backend.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            bool isPasswordValid = VerifyPassword(loginRequest.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var token = GenerateJwtToken(user);

            var expiryInMinutes = _configuration.GetValue<int>("Jwt:ExpiryInMinutes");
            var response = new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(expiryInMinutes)
            };

            return Ok(response);
        }

        [HttpPost("verify-password")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPassword([FromBody] VerifyPasswordRequest verifyPasswordRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByEmailAsync(verifyPasswordRequest.Email);
            if (user == null)
                return Unauthorized(new { message = "Usuario no encontrado." });

            bool isPasswordValid = VerifyPassword(verifyPasswordRequest.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Contraseña incorrecta." });

            return Ok(new { message = "Contraseña correcta." });
        }

        [HttpGet("user-id")]
        [Authorize]
        public IActionResult GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();
            
            return Ok(new { userId = currentUserId });
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings.GetValue<string>("Key");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryInMinutes = jwtSettings.GetValue<int>("ExpiryInMinutes");

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
