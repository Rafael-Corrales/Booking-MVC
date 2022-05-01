using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking_MVC.Models
{
    public class RoomViewModel
    {
        [DisplayName("IdRoom")]
        public int idRoom { get; set; }
        [Required]
        [DisplayName("Room Number")]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int roomNumber { get; set; }


        [DisplayName("Details")]
        public string roomDetails { get; set; }

        [Required]
        [DisplayName("Category")]
        public string categoryRoom { get; set; }

        [Required]
        [DisplayName("Status")]
        public bool roomStatus { get; set; }
    }
}