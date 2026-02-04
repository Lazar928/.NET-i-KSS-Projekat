using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Api.Data;
using Projekat.Api.DTOs.User;

namespace Projekat.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize] // svi endpoint-i traže JWT
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ TEST – bilo koji ulogovani korisnik
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("JWT radi, korisnik je autorizovan ✅");
    }

    // 🔐 ADMIN – LISTA SVIH KORISNIKA
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users
            .Where(u => u.Role != "Admin") // admina ne prikazujemo
            .Select(u => new UserReadDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            })
            .ToList();

        return Ok(users);
    }

    // 🔐 ADMIN – BRISANJE KORISNIKA
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users
            .Include(u => u.Vehicles) // bitno ako owner ima oglase
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound();

        if (user.Role == "Admin")
            return BadRequest("Admin ne može biti obrisan");

        // ako ne želite komplikacije – blokirajte brisanje ownera sa oglasima                ///////DODATI KASNIJE KADA SE SVE ZAVRSI, DODATI DA PROVERAVA KOLIKO OGLASA IMA PRODAVAC I PITA PROZORCIC KOJI PITA ADMINA DA LI JE SIGURAN DA LI ZELI DA OBRISE KORISNIKA JER CE OBRISATI I NJEGOVE OGLASE
        //if (user.Vehicles.Any())                                                            ///////DODATI I SHADOW BAN
            //return BadRequest("Korisnik ima aktivne oglase");

        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok("Korisnik obrisan");
    }

    
}