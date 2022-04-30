using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking_MVC.Models
{
    public class ReservationViewModel
    {
        [DisplayName("Reservation Number")]
        public int idReservation { get; set; }
        [Required]
        [DisplayName("Room")]
        public int idRoom { get; set; }

        [Required]
        [DisplayName("CheckIn Date")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime checkInDate { get; set; }

        [Required]
        [DisplayName("CheckOut Date")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime checkOutDate { get; set; }

        [Required]
        [DisplayName("Booking Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime bookingDate { get; set; }

        [Required]
        [DisplayName("Customer Name")]
        public string customerName { get; set; }

        [Required]
        [DisplayName("Customer Email")]
        [EmailAddress]
        public string email { get; set; }

        [DisplayName("Room")]
        public virtual RoomViewModel room { get; set; }
    }
}