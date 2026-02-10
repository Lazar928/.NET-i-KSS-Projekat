using System.Collections.Generic;

namespace Projekat.Api.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;

    // Owner može imati više vozila
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
