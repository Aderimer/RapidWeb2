using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data;
using Backend.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class AuthService
{
    private readonly DataContext _dataContext;
    private readonly JwtSettings _jwtSettings;

    public AuthService(DataContext dataContext, JwtSettings jwtSettings)
    {
        _dataContext = dataContext;
        _jwtSettings = jwtSettings;
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return false;
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return result == PasswordVerificationResult.Success;
    }

    public async Task<bool> RegisterUserAsync(
        string username,
        string email,
        string password,
        string role = "User"
    )
    {
        if (await _dataContext.Users.AnyAsync(e => e.Email == email))
            return false;

        // Create user
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            Username = username,
            Email = email,
            Role = "User"
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);


        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        #nullable disable
        return await _dataContext.Users.SingleOrDefaultAsync(e => e.Email == email);
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}