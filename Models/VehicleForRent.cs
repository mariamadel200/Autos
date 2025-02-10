namespace Autos.Models
{
    public class VehicleForRent : Vehicle
    {
        public double PricePerDay {  get; set; }

        public virtual List<Rental> ?Rentals { get; set; }

    }
}
