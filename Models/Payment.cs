namespace Autos.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public double TotalPrice {  get; set; }

        public string Method { get; set; }

        public double AmountLeft { get; set; }
    }
}
