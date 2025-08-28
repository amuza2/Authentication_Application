using System.Threading.Tasks;
using AuthenticationApp.Models;

namespace AuthenticationApp.Services;

public interface IAuthenticationService
{
    Task<bool> RegisterUserAsync(string firstName, string lastName, string email, string password);
    Task<User?> LoginUserAsync(string email, string password);
    Task<bool> UserExistsAsync(string email);
}