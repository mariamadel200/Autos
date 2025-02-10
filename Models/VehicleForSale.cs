using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autos.Models
{
    public class VehicleForSale :Vehicle
    {
        public double Price {  get; set; }

        public int? SaleId { get; set; }

        public bool Available { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale? Sale { get; set; }



    }
}
