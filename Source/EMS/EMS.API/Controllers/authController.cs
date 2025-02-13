using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMS.DAL.Interface;
using EMS.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EMS.DAL.Implementation;
using EMS.DataAccess.Models;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly ILoginMgr _loginMgr;
        private readonly ILogger<authController> _logger;

        public authController(ILogger<authController> logger, ILoginMgr loginMgr)
        {
            _logger = logger;
            _loginMgr = loginMgr;
        }

        // api/Authentication/Login
        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDTO model)
        {
            // Authentication
            var result = await _loginMgr.AuthenticateUser(model);

            // Check authentication result
            if(result.ResultCode != 0)
            {
                return BadRequest(result.ResultDescription);
            }

            // Retrieve JWT configuration
            // These lines retrieve the JWT configuration values (Issuer, Audience, and Key) from the application's configuration (e.g., appsettings.json).
            var issuer = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Issuer"];
            var audience = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"]);

            // Create claims
            // Claims are pieces of information about the user. In this case, two claims are created: Name and PrimarySid (Primary Security Identifier) with values from result.UserDTO.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.UserDTO.Name),
              
                new Claim(ClaimTypes.PrimarySid, result.UserDTO.Id.ToString())
            };

            // Token descriptor
            // This object describes the token, including the claims, expiration time (set to 4320 minutes, or 3 days), issuer, audience, and the signing credentials (using the HMAC SHA-512 algorithm).
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // id, name
                Expires = DateTime.UtcNow.AddDays(30), // Adjust the expiration time as needed (30 days)
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            // Generate JWT
            // JwtSecurityTokenHandler is used to create and write the JWT. The token is created based on the tokenDescriptor and then written to a string (jwtToken).
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            // Return JWT
            // If the authentication is successful, the method returns an Ok response with the generated JWT.
            return Ok(new { Token = jwtToken });
        }

        /*
        [AllowAnonymous]
        [HttpGet("IsAuthenticated")]
        public async Task<bool> IsAuthenticated()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity; 
            if (identity != null)
            {
                var currentUserIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(currentUserIdClaim, out int currentUserId))
                {
                    if(currentUserId == 0)
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }

            return false;
        }
        */

        // api/Authentication/Logout
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Since JWT tokens are stateless, you don't need to sign out on the server.
            // Instruct the client to delete the token.
            // Clearing the JWT Token
            return Ok(new { message = "Logged out successfully." }); 
        }
    }
}