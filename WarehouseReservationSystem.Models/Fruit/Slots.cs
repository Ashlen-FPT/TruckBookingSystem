using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.Fruit
{
    public class Slots
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Warehouse ")]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        [Required]
        [Display(Name = "Start Time (24-Hour Time Format ) ")]
        public int StartTime { get; set; }

        [Required]
        [Display(Name = "End Time (24-Hour Time Format )")]
        public int EndTime { get; set; }
    }
}
