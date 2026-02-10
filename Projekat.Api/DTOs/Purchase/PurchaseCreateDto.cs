namespace Projekat.Api.DTOs.Purchase;

public class PurchaseCreateDto
{
    public int VehicleId { get; set; }
}

//Pri pravljenju porudzbine prosledjuje se samo VehicleId a backend popunjava ostalo
