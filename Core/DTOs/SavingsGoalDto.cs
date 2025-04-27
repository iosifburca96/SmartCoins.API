using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCoins.Core.DTOs
{
    public class SavingsGoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime? TargetDate { get; set; }
        public decimal ProgressPercentage => TargetAmount > 0
            ? Math.Min(100, (CurrentAmount / TargetAmount) * 100)
            : 0;
    }
}
