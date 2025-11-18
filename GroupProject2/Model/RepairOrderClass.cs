using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Model
{
    public class RepairOrderClass
    {
        [Key]
        public int RepairOrderId { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public CustomerClass Customer { get; set; }

        
        [ForeignKey(nameof(Technician))]
        public int? TechnicianId { get; set; }
        public TechnicianClass Technician { get; set; }

        public string ApplianceType { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public OrderStatusEnums Status { get; set; }

        // Navigation — no FK here (InvoiceClass owns FK)
        public InvoiceClass Invoice { get; set; }

        public List<OrderPartClass> OrderParts { get; set; } = new();
    }
}
