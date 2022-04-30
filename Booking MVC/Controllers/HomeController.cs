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

        string apiURL = "https://localhost:44359/";
        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            return View();
        }
        // GET: Marca
        public async Task<ActionResult> CheckAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();

            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<RoomViewModel> roomsActive = new List<RoomViewModel>();
            List<RoomViewModel> roomsAvailable = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            string answerReservations = string.Empty;
            
            BetweenDateViewModel betweenDateViewModel = new BetweenDateViewModel { 
                checkInDate = checkInDate,
                checkOutDate = checkOutDate
            };

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                roomsActive = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }
           

            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"reservation/GetReservationsBetwwenDates/{checkInDate}/{checkOutDate}/");
            if (httpResponseMessage2.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage2.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }
          
            if (reservations.Count > 0)
            {
                IEnumerable<int> idRoomsAvailable = reservations.Select(s => s.idRoom).Distinct();
                roomsActive = roomsActive.Where(s => !idRoomsAvailable.Contains(s.idRoom)).ToList();
                
            }

            TimeSpan t = checkInDate - DateTime.Today;
            double nbrOfDays = t.TotalDays;
            if (nbrOfDays > 1)
            {
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

        public async Task<ActionResult> ModifyCheckAvailability(int idReservation, DateTime checkInDate, DateTime checkOutDate)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();

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

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                roomsActive = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }


            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"reservation/GetReservationsBetwwenDatesWithReservation/{checkInDate}/{checkOutDate}/{idReservation}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage2.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }
            if (reservations.Count > 0)
            {
                IEnumerable<int> idRoomsAvailable = reservations.Select(s => s.idRoom).Distinct();
                roomsActive = roomsActive.Where(s => !idRoomsAvailable.Contains(s.idRoom)).ToList();
            }

            ViewBag.BeetweenDatesViewModel = betweenDateViewModel;
            ViewBag.IdReservation = idReservation;
            return View(roomsActive);
        }

        public async Task<ActionResult> Book(int idRoom,DateTime checkInDate, DateTime checkOutDate)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{idRoom}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                myRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }

            ReservationViewModel roomsBooking = new ReservationViewModel 
            {
                idRoom = idRoom,
                checkInDate = checkInDate,
                checkOutDate = checkOutDate,
                room = myRoom
            };

            return View(roomsBooking);
        }




        [HttpPost]
        public async Task<ActionResult> Book(ReservationViewModel reservation)
        {
                reservation.bookingDate = DateTime.Today;
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

        public async Task<ActionResult> BookModify(int idRoom, DateTime checkInDate, DateTime checkOutDate, int idReservation)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;

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

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{idReservation}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            myReservation.idRoom = idRoom;
            myReservation.checkInDate = checkInDate;
            myReservation.checkOutDate = checkOutDate;
            return View(myReservation);
        }
        [HttpPost]
        public async Task<ActionResult> BookModify(ReservationViewModel reservation)
        {
            reservation.bookingDate = DateTime.Today;
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


        public async Task<ActionResult> MyReservations()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            string answerReservations = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await (httpResponseMessage.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }

            return View(reservations);
        }


        public async Task<ActionResult> ModifyReservation(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyReservation(ReservationViewModel myReservation)
        {

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PutAsync(apiURL + $"reservation/Update", new StringContent(
    JsonConvert.SerializeObject(myReservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }         
            return View();
        }
        public async Task<ActionResult> Detail(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }

        public async Task<ActionResult> CancelReservation(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await (httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }

        [HttpPost]
        public async Task<ActionResult> CancelReservation(ReservationViewModel myReservation)
        {
                HttpClient httpClient = new HttpClient();
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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}