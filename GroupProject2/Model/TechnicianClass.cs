using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject2.Model
{
    public class TechnicianClass
    {
        [Key]
        public int TechnicianId { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }

        public List<RepairOrderClass> RepairOrders { get; set; } = new();




    }
}
