
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VeterinaryClinic.Application.DTOs.Auth;
using VeterinaryClinic.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using VeterinaryClinic.Domain.Exceptions;


namespace VeterinaryClinic.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            
            _logger = logger;
        }

        public async Task<AuthResponseDto> ResgiterAsync(RegisterDto dto)
        {
            _logger.LogInformation("Registering new user with email: {Email}", dto.Email);
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email.ToLower());
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed. User with email {Email} already exists.", dto.Email);
                throw new BusinessRuleException("EmailAlreadyExists", "A user with this email already exists.");
            }
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = dto.Role,
                CreateAt = DateTime.UtcNow,
                IsActive = true

            };

            await _unitOfWork.Users.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("User with email {Email} registered successfully.", dto.Email);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Token = token
            };
        }
        public string GenerateJwtToken(User user)
        {

            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER environment variable is not set.");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE environment variable is not set.");


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("User attempting to log in with email: {Email}", loginDto.Email);

            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email.ToLower());
            if (user == null)
            {
                _logger.LogWarning("Login failed. User with email {Email} not found.", loginDto.Email);
                throw new BusinessRuleException("InvalidCredentials" ,"Invalid email or password.");                
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed. User with email {Email} is inactive.", loginDto.Email);
                throw new BusinessRuleException("AccountDisable","User account is inactive.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed. Invalid password for email {Email}.", loginDto.Email);
                throw new BusinessRuleException("InvalidCredentials","Invalid email or password.");                
            }

            _logger.LogInformation("User with email {Email} logged in successfully.", loginDto.Email);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Token = token
            };
            

        }
    }


}