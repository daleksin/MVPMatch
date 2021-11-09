namespace MVPMatch.Application.Models.Products
{
    public class UpsertProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public decimal CostEur { get; set; }
    }
}
