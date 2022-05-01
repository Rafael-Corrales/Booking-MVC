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
    public class ReservationController : Controller
    {
        // Set the API URL
        string apiURL = "https://localhost:44359/";
        #region Index
        /// <summary>
        ///  Returns the Index Reservation View
        /// </summary>
        public async Task<ActionResult> Index()
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            // Create the variables
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            string answerReservations = string.Empty;

            // Send the HTP request to get all reservations
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await(httpResponseMessage.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }

            return View(reservations);
        }
        #endregion
        #region Create View
        /// <summary>
        ///  Returns the Create Reservation View
        /// </summary>
        public async Task<ActionResult> Create()
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            // Create the variables
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            // Send the HTP request to get the actives rooms
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }
            ViewBag.Rooms = rooms;
            return View();
        }

        #region Create Post Form
        /// <summary>
        ///  Create Reservation By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create(ReservationViewModel reservation)
        {
 
             reservation.bookingDate = DateTime.Today;
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            // Send the HTP request to insert a reservation
            var response = await httpClient.PostAsync(apiURL + $"reservation/Insert", new StringContent(
    JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            return View();
        }
        #endregion

        #region Edit View
        /// <summary>
        ///  Returns the Edit Reservation View
        /// </summary>
        public async Task<ActionResult> Edit(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            // Create the variables
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;
            // Send the HTP request to get an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await(httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }


            HttpClient httpClient2 = new HttpClient();
            HttpResponseMessage httpResponseMessage2 = new HttpResponseMessage();
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            // Send the HTP request to get the actives rooms
            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage2.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage2.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }
            ViewBag.Rooms = rooms;

            return View(myReservation);
        }
        #endregion

        #region Edit Post Form
        /// <summary>
        ///  Edit Reservation By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Edit(ReservationViewModel myReservation)
        {
            myReservation.bookingDate = DateTime.Today;
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            // Send the HTP request to update an specific reservation
            var response = await httpClient.PutAsync(apiURL + $"reservation/Update", new StringContent(
    JsonConvert.SerializeObject(myReservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            return View();
        }
        #endregion

        #region Detail View
        /// <summary>
        ///  Returns the Detail Reservation View
        /// </summary>
        public async Task<ActionResult> Detail(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;
            // Send the HTP request to get an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await(httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }
        #endregion

        #region Delete View
        /// <summary>
        ///  Returns the Delete Reservation View
        /// </summary>
        public async Task<ActionResult> Delete(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;
            // Send the HTP request to get an specific reservation by IdReservation
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await(httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }
        #endregion

        #region Delete Form Post
        /// <summary>
        ///  Delete Reservation By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Delete(ReservationViewModel myReservation)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            // Send the HTP request to delete an specific reservation
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
            return View();
        }
        #endregion

    }
}