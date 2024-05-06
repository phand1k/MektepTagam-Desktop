using MektepTagam.Interfaces;
using MektepTagam.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MektepTagam.Data
{
    public class CardCode : IEntityString
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DateOfCreated { get; set; } = DateTime.Now;
        public string? AspNetUserId { get; set; }
        public CardCode()
        {
        }
    }
}
