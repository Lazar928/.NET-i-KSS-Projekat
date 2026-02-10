namespace Projekat.Api.DTOs.Purchase;

public class PurchaseReadDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = null!;
    public string BuyerUsername { get; set; } = null!;
    public string OwnerUsername { get; set; }

    public string BuyerEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}

//Izlazni DTO, sluzi za vracanje podataka o kupovinama i sadrzi informacije koje su potrebne za prikaz 
