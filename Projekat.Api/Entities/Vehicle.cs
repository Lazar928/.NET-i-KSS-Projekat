namespace Projekat.Api.Entities;

public class Vehicle
{
    public int Id { get; set; } //pk
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;

    // VEZA KA OWNER-U
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}
