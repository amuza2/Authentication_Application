using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApp.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required] [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required] [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required] [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    
}