using Booking_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Booking_MVC.Controllers
{
    public class HomeController : Controller
    {
        // Set the API URL
        string apiURL = "https://localhost:44359/";

        #region Index
        /// <summary>
        ///  Returns the View Home Index
        /// </summary>
        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            return View();
        }
        #endregion

        #region Check Availability
        /// <summary>
        ///  Returns the Check Availability View
        /// </summary>
        public async Task<ActionResult> CheckAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();

            // Create the variables
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<RoomViewModel> roomsActive = new List<RoomViewModel>();
            List<RoomViewModel> roomsAvailable = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            string answerReservations = string.Empty;
           
            BetweenDateViewModel betweenDateViewModel = new BetweenDateViewModel { 
                checkInDate = checkInDate,
                checkOutDate = checkOutDate
            };

            // Send the HTP request to get the Actives Room
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                roomsActive = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }

            // Send the HTP request to get the Reservations between two dates
            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"reservation/GetReservationsBetwwenDates/{checkInDate}/{checkOutDate}/");
            if (httpResponseMessage2.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage2.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }
          
            // If there are more than one reservations in those dates, the rooms of that reservations won't be available
            if (reservations.Count > 0)
            {
                IEnumerable<int> idRoomsAvailable = reservations.Select(s => s.idRoom).Distinct();
                roomsActive = roomsActive.Where(s => !idRoomsAvailable.Contains(s.idRoom)).ToList();
                
            }

            // Get the days of difference to meet this requirement: "All reservations start at least the next day of booking"
            TimeSpan t = checkInDate - DateTime.Today;
            double nbrOfDays = t.TotalDays;
            if (nbrOfDays > 1)
            {
                // If the day of diference is more than 1, it will show no rooms available
                roomsActive = roomsActive.Where(s => s.idRoom < 0).ToList();
                ViewBag.Message = "Y";
            }
            else
            {
                ViewBag.Message = "N";
            }
            
            ViewBag.BeetweenDatesViewModel = betweenDateViewModel;
            return View(roomsActive);
        }
        #endregion

        #region Check Availability in Modify Option
        /// <summary>
        ///  Returns the Check Availability in Modify Option View
        /// </summary>
        public async Task<ActionResult> ModifyCheckAvailability(int idReservation, DateTime checkInDate, DateTime checkOutDate)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
            // Create the variables
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<RoomViewModel> roomsActive = new List<RoomViewModel>();
            List<RoomViewModel> roomsAvailable = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            string answerReservations = string.Empty;

            BetweenDateViewModel betweenDateViewModel = new BetweenDateViewModel
            {
                checkInDate = checkInDate,
                checkOutDate = checkOutDate
            };

            // Send the HTP request to get the Actives Room
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                roomsActive = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }

            // Send the HTP request to get the Reservations between two dates
            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"reservation/GetReservationsBetwwenDatesWithReservation/{checkInDate}/{checkOutDate}/{idReservation}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage2.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }

            // If there are more than one reservations in those dates, the rooms of that reservations won't be available
            if (reservations.Count > 0)
            {
                IEnumerable<int> idRoomsAvailable = reservations.Select(s => s.idRoom).Distinct();
                roomsActive = roomsActive.Where(s => !idRoomsAvailable.Contains(s.idRoom)).ToList();
            }

            // Get the days of difference to meet this requirement: "All reservations start at least the next day of booking"
            TimeSpan t = checkInDate - DateTime.Today;
            double nbrOfDays = t.TotalDays;
            if (nbrOfDays > 1)
            {
                // If the day of diference is more than 1, it will show no rooms available
                roomsActive = roomsActive.Where(s => s.idRoom < 0).ToList();
                ViewBag.Message = "Y";
            }
            else
            {
                ViewBag.Message = "N";
            }

            ViewBag.BeetweenDatesViewModel = betweenDateViewModel;
            ViewBag.IdReservation = idReservation;
            return View(roomsActive);
        }
        #endregion

        #region Book View
        /// <summary>
        ///  Returns the Book View
        /// </summary>
        public async Task<ActionResult> Book(int idRoom,DateTime checkInDate, DateTime checkOutDate)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;
            // Send the HTP request to get the an specific room by IdRoom
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{idRoom}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                myRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }

            // The values that will show in the view
            ReservationViewModel roomsBooking = new ReservationViewModel 
            {
                idRoom = idRoom,
                checkInDate = checkInDate,
                checkOutDate = checkOutDate,
                room = myRoom
            };

            return View(roomsBooking);
        }
        #endregion


        #region Book the reservation submitted by Post Method
        /// <summary>
        ///  Book the reservation submitted by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Book(ReservationViewModel reservation)
        {
            // Set the booking Date to Today
                reservation.bookingDate = DateTime.Today;
            // Create an httpClient to send the requests to the API
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(apiURL + $"reservation/Insert", new StringContent(
    JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
        }
        #endregion
        #region Book View
        /// <summary>
        ///  Returns the Book View
        /// </summary>
        public async Task<ActionResult> BookModify(int idRoom, DateTime checkInDate, DateTime checkOutDate, int idReservation)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;
            // Send the HTP request to get the an specific room by IdRoom
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{idRoom}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                myRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }

            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;
            // Send the HTP request to get the an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{idReservation}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            // The values that will show in the view
            myReservation.idRoom = idRoom;
            myReservation.checkInDate = checkInDate;
            myReservation.checkOutDate = checkOutDate;
            return View(myReservation);
        }
        #endregion

        #region Book the reservation submitted by Post Method
        /// <summary>
        ///  Book the reservation submitted by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> BookModify(ReservationViewModel reservation)
        {
            reservation.bookingDate = DateTime.Today;
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PutAsync(apiURL + $"reservation/Update", new StringContent(
JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        #endregion


        #region My Reservations
        /// <summary>
        ///  Get the reservations
        /// </summary>
        public async Task<ActionResult> MyReservations()
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            string answerReservations = string.Empty;

            // Send the HTP request to get all the reservations
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }

            return View(reservations);
        }
        #endregion

        #region Modify Reservation End-User
        /// <summary>
        ///  Returns the Modify Reservation End-User View
        /// </summary>
        public async Task<ActionResult> ModifyReservation(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            // Send the HTP request to get the an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }
        #endregion

        #region Modify Reservation End-User Form Submited
        /// <summary>
        ///  Modify Reservation End-User Form Submited by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ModifyReservation(ReservationViewModel myReservation)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();

            // Send the HTP request to update the an specific reservation by IdReservation
            var response = await httpClient.PutAsync(apiURL + $"reservation/Update", new StringContent(
    JsonConvert.SerializeObject(myReservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }         
            return View();
        }
        #endregion

        #region Cancel Reservation View
        /// <summary>
        ///  Returns the Cancel Reservation View
        /// </summary>
        public async Task<ActionResult> CancelReservation(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            // Send the HTP request to get the an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }
        #endregion
        #region Cancel Reservation Submitted Form
        /// <summary>
        ///  Cancel Reservation By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CancelReservation(ReservationViewModel myReservation)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();

            // Send the HTP request to delete an especific reservation
            var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiURL + $"reservation/Delete"),
                    Content = new StringContent(JsonConvert.SerializeObject(myReservation), Encoding.UTF8, "application/json")
                };
                var responseSend = await httpClient.SendAsync(request);
                if (responseSend.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }

        }
        #endregion
    }
}