using System;

namespace Projekat.Api.Entities;

public class Purchase
{
    public int Id { get; set; }

    public int BuyerId { get; set; }
    public User Buyer { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}