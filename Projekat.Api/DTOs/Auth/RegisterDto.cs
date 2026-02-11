namespace Projekat.Api.DTOs.Auth;
using System.ComponentModel.DataAnnotations;


public class RegisterDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; } // Owner | Buyer
}


// Frontend salje podatke za pravljenje novog korisnika
// Kasnije smo u frontendu ogranicili da u rolu ne moze da izabere admina
