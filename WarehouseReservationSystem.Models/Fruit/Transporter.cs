using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.Fruit
{
    public class Transporter
    {
        public int Id { get; set; }

        [Display(Name = "Key Code")]
        [Required]
        public string KeyCode { get; set; }

        [Display(Name = "Transporter")]
        [Required]
        public string Name { get; set; }
    }
}
