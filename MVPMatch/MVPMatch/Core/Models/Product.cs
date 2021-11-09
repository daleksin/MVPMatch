namespace MVPMatch.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int AmountAvailable { get; set; }
        public decimal CostEur { get; set; }
        public string ProductName { get; set; }
        public string SellerId { get; set; }
    }
}
