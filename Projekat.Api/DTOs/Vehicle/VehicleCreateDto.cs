namespace Projekat.Api.DTOs.Vehicle
{
    public class VehicleCreateDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
