using MektepTagam.Data;
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
    public class TransactionMT : IEntityString
    {
        public Guid Id { get; set; }
        public double? Amount { get; set; }
        [ForeignKey("CardCodeId")]
        public Guid? CardCodeId { get; set; }
        public CardCode? CardCode { get; set; }
        public bool? IsSent { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;
        [ForeignKey("DishId")]
        public Guid? DishId { get; set; }
        public Dish? Dish { get; set; }
        public DateTime? DateOfCreatedTransaction { get; set; } = DateTime.Now;
    }
}
