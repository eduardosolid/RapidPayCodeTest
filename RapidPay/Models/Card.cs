namespace RapidPay.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public long Number { get; set; }
        public decimal Balance { get; set; }
    }
}
