using MVPMatch.Core.Models;

namespace MVPMatch.Application.Models.Actions
{
    public class BuyResultDto
    {
        public decimal MoneySpentEur { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public ChangeDto Change { get; set; }
    }
}
