using System.Collections.Generic;

namespace MVPMatch.Application.Models.Actions
{
    public class ChangeDto
    {
        public Dictionary<int, int> Coins { get; set; } = new();
    }
}
