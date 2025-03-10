using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using CloudBased_TaskManagementAPI.Data;
using CloudBased_TaskManagementAPI.DTOs;
using CloudBased_TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CloudBased_TaskManagementAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            // Name      : AuthService(AppDbContext context, IConfiguration config)
            // Purpose   : Initializes authentication service with database context and configuration.
            // Re-use    : Injected into controllers for authentication operations.
            // Input     : AppDbContext context
            //            - Database context for user authentication.
            //             IConfiguration config
            //            - Configuration settings for JWT.
            // Output    : Initializes private fields _context and _config.
            _context = context;
            _config = config;
        } // end constructor

        public async Task<string?> RegisterUserAsync(RegisterRequest request)
        {
            // Name      : RegisterUserAsync(RegisterRequest request)
            // Purpose   : Registers a new user by storing hashed password in the database.
            // Re-use    : Called when a new user signs up.
            // Input     : RegisterRequest request
            //            - Contains username and password.
            // Output    : JWT token if registration is successful and null if username already exists.
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return null;

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return GenerateJwtToken(user);
        } // end RegisterUserAsync

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            // Name      : LoginAsync(LoginRequest request)
            // Purpose   : Authenticates user by verifying credentials.
            // Re-use    : Used when a user logs in.
            // Input     : LoginRequest request
            //            - Contains username and password.
            // Output    : JWT token if authentication is successful; null otherwise.
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == request.Username
            );
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;
            return GenerateJwtToken(user);
        } // end LoginAsync

        private string GenerateJwtToken(User user)
        {
            // Name      : GenerateJwtToken(User user)
            // Purpose   : Generates a JWT token for the authenticated user.
            // Re-use    : Called during user registration and login.
            // Input     : User user
            //            - The authenticated user.
            // Output    : JWT token string.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing")
                )
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        } // end GenerateJwtToken
    } // end AuthService
}
