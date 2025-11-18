using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Model
{
    public class RepairPartClass
    {
        [Key]
        public int PartId { get; set; }

        public string PartName { get; set; }
        public decimal UnitPrice { get; set; }

        public List<OrderPartClass> OrderParts { get; set; } = new();
    }
}
