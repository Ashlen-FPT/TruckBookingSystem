using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.Fruit
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Display(Name = "Warehouse")]
        [Required]

        public string Name { get; set; }

        [Display(Name = "Number Of Slots")]
        [Required]
        public int NumOfSlots { get; set; }

        [Display(Name = "Number Of Slots GC")]
        [Required]
        public int GCSlots { get; set; }

        [Display(Name = "Location ID")]
        [Required]
        public string LocationId { get; set; }


    }

}
