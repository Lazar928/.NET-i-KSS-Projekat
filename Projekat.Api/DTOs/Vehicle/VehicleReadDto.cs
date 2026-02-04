namespace Projekat.Api.DTOs.Vehicle
{
    public class VehicleReadDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string OwnerUsername { get; set; }
        public int OwnerId { get; set; }
    }
}

