using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthenticationApp.Data;
using AuthenticationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApplicationDbContext _context;

    public AuthenticationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterUserAsync(string firstName, string lastName, string email, string password)
    {
        try
        {
            if (await UserExistsAsync(email))
                return false;
            
            var passwordHash = HashPassword(password);
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email.ToLowerInvariant(),
                PasswordHash = passwordHash,
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bashBytes);
    }

    public async Task<User?> LoginUserAsync(string email, string password)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;

        }
        catch
        {
            return null;
        }
    }

    private bool VerifyPassword(string password, string dbHash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == dbHash;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email.ToLowerInvariant());
    }
}