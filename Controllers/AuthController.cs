using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Identity.Data;

/// <summary>
/// Authentication endpoints for obtaining JWT tokens.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Authenticate a user and return a JWT token on success.
    /// </summary>
    /// <param name="request">Login request containing Username and Password.</param>
    /// <returns>JWT token and basic user info when credentials are valid; 401 Unauthorized otherwise.</returns>
    [HttpPost("login")]
    public IActionResult Login([FromBody] omech.Models.LoginRequests request)
    {
        string connectionString = _configuration.GetConnectionString("db_dev_con");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SP_LOGIN_USER", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", request.Username);
            cmd.Parameters.AddWithValue("@Password", request.Password);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                int userId = Convert.ToInt32(reader["USER_SRNO"]);
                string username = reader["USERNAME"].ToString();
                int role = Convert.ToInt32(reader["USER_TYPE_SRNO"]);

                // Generate JWT Token
                var token = GenerateJwtToken(userId, username, role);
                return Ok(new
                {
                    Token = token,
                    User = new { userId, username, role }
                });
            }
            else
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }
    }

    /// <summary>
    /// Generate a signed JWT token for an authenticated user.
    /// </summary>
    private string GenerateJwtToken(int userId, string username, int role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
