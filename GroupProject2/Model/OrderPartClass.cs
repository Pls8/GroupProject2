using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Model
{
    public class OrderPartClass
    {
        [Key]
        public int OrderPartId { get; set; }

        // FK → RepairOrder
        
        [ForeignKey(nameof(RepairOrder))]
        public int RepairOrderId { get; set; }
        public RepairOrderClass RepairOrder { get; set; }

        // FK → RepairPart
        
        [ForeignKey(nameof(RepairPart))]
        public int PartId { get; set; }
        public RepairPartClass RepairPart { get; set; }

        // Quantity of part used
        public int Quantity { get; set; }
    }
}
