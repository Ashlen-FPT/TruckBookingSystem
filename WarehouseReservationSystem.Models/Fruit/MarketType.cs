using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.Fruit
{
    public class MarketType
    {

        public int Id { get; set; }

        [Display(Name = "Market Type")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}
