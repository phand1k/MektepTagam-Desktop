using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MektepTagam.Data
{
    public class Token
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public string? AspNetUserId { get; set; }
        public DateTime? DateOfEndToken { get; set; }
        public int? OrganizationId { get; set; }
        public string? Role { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public DateTime? DateOfLogin { get; set; } = DateTime.Now;
    }
}
