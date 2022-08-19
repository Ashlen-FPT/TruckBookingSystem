using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.Fruit
{
    public class Exporter
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Key Code")]
        public string KeyCode { get; set; }

        [Display(Name = "Exporter")]
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public Boolean EmailActive { get; set; }

        public Boolean IsActive { get; set; }

        public Exporter()
        {
            this.IsActive = true;
            this.EmailActive = false;

        }
    }
}
