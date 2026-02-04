using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Api.Data;
using Projekat.Api.DTOs.Vehicle;
using Projekat.Api.Entities;
using System.Security.Claims;

namespace Projekat.Api.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehicleController : ControllerBase
{
    private readonly AppDbContext _context;

    public VehicleController(AppDbContext context)
    {
        _context = context;
    }

    // 🔓 GET ALL – svi
    [HttpGet]
    public IActionResult GetAll()
    {
        var vehicles = _context.Vehicles
            .Include(v => v.Owner)
            .Select(v => new VehicleReadDto
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                Price = v.Price,
                Description = v.Description,
                OwnerUsername = v.Owner.Username,
                OwnerId = v.OwnerId
            })
            .ToList();

        return Ok(vehicles);
    }

    // 🔓 GET BY ID – svi
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var v = _context.Vehicles
            .Include(x => x.Owner)
            .FirstOrDefault(x => x.Id == id);

        if (v == null) return NotFound();

        return Ok(new VehicleReadDto
        {
            Id = v.Id,
            Brand = v.Brand,
            Model = v.Model,
            Year = v.Year,
            Price = v.Price,
            Description = v.Description,
            OwnerUsername = v.Owner.Username
        });
    }

    // 🔐 CREATE – samo OWNER
    [Authorize(Roles = "Owner")]
    [HttpPost]
    public IActionResult Create(VehicleCreateDto dto)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var owner = _context.Users.First(u => u.Username == username);

        var vehicle = new Vehicle
        {
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            Price = dto.Price,
            Description = dto.Description,
            OwnerId = owner.Id
        };

        _context.Vehicles.Add(vehicle);
        _context.SaveChanges();

        return Ok("Vozilo dodato");
    }

    // 🔐 UPDATE – samo OWNER (svoje)
    [Authorize(Roles = "Owner")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, VehicleUpdateDto dto)
    {
        var vehicle = _context.Vehicles
            .Include(v => v.Owner)
            .FirstOrDefault(v => v.Id == id);

        if (vehicle == null) return NotFound();

        var username = User.FindFirstValue(ClaimTypes.Name);
        if (vehicle.Owner.Username != username)
            return Forbid();

        vehicle.Brand = dto.Brand;
        vehicle.Model = dto.Model;
        vehicle.Year = dto.Year;
        vehicle.Price = dto.Price;
        vehicle.Description = dto.Description;

        _context.SaveChanges();
        return Ok("Vozilo izmenjeno");
    }

    // 🔐 DELETE – OWNER i ADMIN (Owner samo svoje, Admin sve)
    [Authorize(Roles = "Owner,Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var vehicle = _context.Vehicles
            .Include(v => v.Owner)
            .FirstOrDefault(v => v.Id == id);

        if (vehicle == null)
            return NotFound();

        var username = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        // ADMIN može sve
        if (role != "Admin")
        {
            // OWNER može samo svoje
            if (vehicle.Owner.Username != username)
                return Forbid();
        }

        _context.Vehicles.Remove(vehicle);
        _context.SaveChanges();

        return Ok("Vozilo obrisano");
    }

    // DELETE da adming moze da brise od svakod Owner-a
    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/{id}")]
    public IActionResult DeleteByAdmin(int id)
    {
        var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);

        if (vehicle == null)
            return NotFound("Vozilo ne postoji");

        _context.Vehicles.Remove(vehicle);
        _context.SaveChanges();

        return Ok();
    }   
}