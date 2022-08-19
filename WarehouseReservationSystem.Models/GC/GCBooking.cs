using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseReservationSystem.Models.Fruit;

namespace WarehouseReservationSystem.Models.GC
{
    public class GCBooking
    {

        public int Id { get; set; }

        [Required]
        //[Index(IsUnique = true)]
        [StringLength(10)]
        public string BookingRef { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Warehouse ")]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        [Required]
        [Display(Name = "Vessel ")]
        public int VesselId { get; set; }

        [ForeignKey("VesselId")]
        public Vessel Vessel { get; set; }

        [Required]
        [Display(Name = "Customer ")]
        public int ExporterId { get; set; }

        [ForeignKey("ExporterId")]
        public Exporter Exporter { get; set; }

        [Required]
        [Display(Name = "Transporter ")]
        public int TransporterId { get; set; }

        [ForeignKey("TransporterId")]
        public Transporter Transporter { get; set; }

        [Required]
        [Display(Name = "Truck Registration ")]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9]+$", ErrorMessage = "Please enter a valid Registration(remove spaces & special characters if any).")]
        //[RegularExpression(@"[^\s]+", ErrorMessage = "Please remove spaces.")]
        public string Registration { get; set; }

        [Display(Name = "Trailer Registration ")]
        //[RegularExpression(@"[^\s]+", ErrorMessage = "Please remove spaces.")]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9]+$", ErrorMessage = "Please enter a valid Registration(remove spaces & special characters if any).")]
        public string TrailerReg { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public bool Import { get; set; }
        [Required]
        public bool Export { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ']+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone No. ")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Time (Hour - 24Hr) ")]
        public int Time { get; set; }

        [Required]
        [Display(Name = "Status ")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDateUtc { get; set; }

        public GCBooking()
        {
            this.CreatedDateUtc = DateTime.Now;
            this.Import = false;
            this.Export = false;
        }

    }
}
