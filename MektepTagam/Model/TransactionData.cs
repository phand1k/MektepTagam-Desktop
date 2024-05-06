using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MektepTagam.Model
{
    public class TransactionData
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public Guid CardCodeId { get; set; }
        public Guid DishId { get; set; }
        public DateTime? DateOfCreatedTransaction { get; set; } = DateTime.Now;
    }
}
