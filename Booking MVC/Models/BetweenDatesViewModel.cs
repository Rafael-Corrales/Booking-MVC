using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking_MVC.Models
{
    public class BetweenDateViewModel
    {

        [Required]
        [DisplayName("CheckIn Date")]
        public DateTime checkInDate { get; set; }

        [Required]
        [DisplayName("CheckOut Date")]
        public DateTime checkOutDate { get; set; }

    }
}