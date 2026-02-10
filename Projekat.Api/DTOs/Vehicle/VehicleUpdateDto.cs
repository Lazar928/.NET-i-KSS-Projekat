namespace Projekat.Api.DTOs.Vehicle
{
    public class VehicleUpdateDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } //Nije ubaceno u funkciju, ali je ostavljeno za prosirenje
    }
}

//Ulazni DTO, sluzi za izmenu vozila i sadrzi samo podatke koji su dozvoljeni da se menjaju
