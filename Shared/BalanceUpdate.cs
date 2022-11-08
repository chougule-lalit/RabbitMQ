using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BalanceUpdate
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
