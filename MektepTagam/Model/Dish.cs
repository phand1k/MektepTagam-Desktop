using MektepTagam.Interfaces;
using MektepTagam.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MektepTagam.Model
{
    public class Dish : IEntityString
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public ICollection<TransactionMT> Transactions { get; set; }
        public Dish()
        {
            Transactions = new List<TransactionMT>();
        }
    }
}
