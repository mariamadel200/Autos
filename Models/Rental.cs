using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autos.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

//        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }
        public int PaymentId { get; set; }
        public bool Delivered { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }

        public virtual List<VehicleForRent>? VehiclesForRents { get; set; }
    }
}

