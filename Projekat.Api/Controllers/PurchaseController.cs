using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Api.Data;
using Projekat.Api.DTOs.Purchase;
using Projekat.Api.Entities;
using System.Security.Claims;

namespace Projekat.Api.Controllers;

[ApiController]
[Route("api/purchases")]
[Authorize]
public class PurchaseController : ControllerBase
{
    private readonly AppDbContext _context; //veza sa bazom

    public PurchaseController(AppDbContext context)
    {
        _context = context;
    }

    // ============================
    // BUYER – KREIRANJE PORUDŽBINE
    // ============================
    [Authorize(Roles = "Buyer")]
    [HttpPost]
    public IActionResult CreatePurchase(PurchaseCreateDto dto)
    {
        var buyerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // proveri da li vozilo postoji
        var vehicle = _context.Vehicles
            .Include(v => v.Owner)
            .FirstOrDefault(v => v.Id == dto.VehicleId);

        if (vehicle == null)
            return NotFound("Vozilo ne postoji");

        // ❌ buyer ne može 2x za isto vozilo
        var alreadyExists = _context.Purchases
            .Any(p => p.BuyerId == buyerId && p.VehicleId == dto.VehicleId);

        if (alreadyExists)
            return BadRequest("Već ste poslali porudžbinu za ovo vozilo");

        var purchase = new Purchase
        {
            BuyerId = buyerId,
            VehicleId = dto.VehicleId
        };

        _context.Purchases.Add(purchase);
        _context.SaveChanges();

        return Ok("Porudžbina je poslata. Prodavac će vas uskoro kontaktirati.");
    }

    // ============================
    // BUYER – MOJE PORUDŽBINE
    // ============================
    [Authorize(Roles = "Buyer")]
    [HttpGet("my")]
    public IActionResult GetMyPurchases()
    {
        var buyerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var purchases = _context.Purchases
            .Include(p => p.Vehicle)
                .ThenInclude(v => v.Owner)   
            .Include(p => p.Buyer)
            .Where(p => p.BuyerId == buyerId)
            .Select(p => new PurchaseReadDto
            {
                Id = p.Id,
                VehicleId = p.VehicleId,
                VehicleName = p.Vehicle.Brand + " " + p.Vehicle.Model,
                BuyerUsername = p.Buyer.Username,
                OwnerUsername = p.Vehicle.Owner.Username, 
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return Ok(purchases);
    }

    // ============================
    // OWNER – PORUDŽBINE ZA MOJA VOZILA
    // ============================
    [Authorize(Roles = "Owner")]
    [HttpGet("for-my-vehicles")]
    public IActionResult GetPurchasesForMyVehicles()
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var purchases = _context.Purchases
            .Include(p => p.Vehicle)
            .Include(p => p.Buyer)
            .Where(p => p.Vehicle.OwnerId == ownerId)
            .Select(p => new PurchaseReadDto
            {
                Id = p.Id,
                VehicleId = p.VehicleId,
                VehicleName = p.Vehicle.Brand + " " + p.Vehicle.Model,
                BuyerUsername = p.Buyer.Username,
                BuyerEmail = p.Buyer.Email,
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return Ok(purchases);
    }

    // ============================
    // ADMIN – SVE PORUDŽBINE
    // ============================
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAllPurchases()
    {
        var purchases = _context.Purchases
            .Include(p => p.Vehicle)
            .Include(p => p.Buyer)
            .Select(p => new PurchaseReadDto
            {
                Id = p.Id,
                VehicleId = p.VehicleId,
                VehicleName = p.Vehicle.Brand + " " + p.Vehicle.Model,
                BuyerUsername = p.Buyer.Username,
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return Ok(purchases);
    }
}
