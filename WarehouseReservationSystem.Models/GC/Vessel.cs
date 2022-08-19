using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseReservationSystem.Models.GC
{
    public class Vessel
    {
        public int Id { get; set; }

        public string VesselNo { get; set; }

        public string VoyageNo { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Vessel()
        {
            IsActive = true;
        }

    }

}
