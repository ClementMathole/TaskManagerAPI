using CloudBased_TaskManagementAPI.DTOs;
using CloudBased_TaskManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudBased_TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
         //Name      : AuthController
         // Purpose   : Constructor to initialize the authentication service via dependency injection.
         // Re-use    : Used during the instantiation of the controller.
         // Input     : AuthService authService
         //            - authentication service instance.
         // Output    : Sets the private variable _authService.
            _authService = authService;
        } // end Constractor

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
         // Name      : Register([FromBody] RegisterRequest request)
         // Purpose   : Registers a new user and returns an authentication token upon success.
         // Re-use    : Called when a new user wants to register.
         // Input     : RegisterRequest request
         //            - user registration details.
         // Output    : Returns an authentication token or a BadRequest response.
            var token = await _authService.RegisterUserAsync(request);
            if (token == null)
            {
                return BadRequest("Username already exists.");
            }
            return Ok(new { Token = token });
        } // end Register

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Name      : Login([FromBody] LoginRequest request)
            // Purpose   : Authenticates a user and returns an authentication token upon success.
            // Re-use    : Called when an existing user wants to log in.
            // Input     : LoginRequest request
            //            - user login details.
            // Output    : Returns an authentication token or an Unauthorized response.
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { Token = token });
        } // end Login
    } // end AuthController
}
