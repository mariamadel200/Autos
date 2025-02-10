using System.ComponentModel.DataAnnotations.Schema;

namespace Autos.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int PaymentId {  get; set; }
        public DateTime SaleDate { get; set; }  

        public bool Delivered { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }

        public virtual List<VehicleForSale> VehiclesForSales { get; set; }
    }
}
