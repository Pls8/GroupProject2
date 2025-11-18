using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Model
{
    public class InvoiceClass
    {
        [Key]
        public int InvoiceId { get; set; }

        // Foreign Key to RepairOrder
        

        [ForeignKey(nameof(RepairOrder))]
        public int RepairOrderId { get; set; }
        public RepairOrderClass RepairOrder { get; set; }

        public decimal ServiceCost { get; set; }
        public decimal PartsCost { get; set; }

        public decimal Total => ServiceCost + PartsCost;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

    }
}
