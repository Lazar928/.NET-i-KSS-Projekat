namespace Projekat.Api.DTOs.Auth;

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

//Predstavlja podatke koje frontend salje backendu prilikom logovanja
