using System;

namespace Projekat.Api.Entities;

public class Purchase
{
    public int Id { get; set; } // pk

    public int BuyerId { get; set; } //fk
    public User Buyer { get; set; } = null!;

    public int VehicleId { get; set; } //fk
    public Vehicle Vehicle { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
