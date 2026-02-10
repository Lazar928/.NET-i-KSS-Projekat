namespace Projekat.Api.DTOs.Auth;

public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Owner | Buyer
}

//Frontend salje podatke za pravljenje novog korisnika
// Kasnije smo u frontendu ogranicili da korisnik ne moze da izabere rolu admin pri registraciji
