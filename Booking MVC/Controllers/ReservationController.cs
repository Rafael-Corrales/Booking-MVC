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
        string apiURL = "https://localhost:44359/";
        // GET: Marca
        public async Task<ActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            string answerReservations = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservations = await(httpResponseMessage.Content.ReadAsStringAsync());
                reservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(answerReservations);
            }

            return View(reservations);
        }
        public async Task<ActionResult> Create()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }
            ViewBag.Rooms = rooms;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ReservationViewModel reservation)
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
               
            
            return View();
        }
        public async Task<ActionResult> Edit(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

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

            httpResponseMessage2 = await httpClient2.GetAsync(apiURL + $"room/GetActives");
            if (httpResponseMessage2.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage2.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);
            }
            ViewBag.Rooms = rooms;

            return View(myReservation);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ReservationViewModel myReservation)
        {
                myReservation.bookingDate = DateTime.Today;
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PutAsync(apiURL + $"reservation/Update", new StringContent(
    JsonConvert.SerializeObject(myReservation), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }

            return View();
        }
        public async Task<ActionResult> Detail(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await(httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            ReservationViewModel myReservation = new ReservationViewModel();
            string answerReservation = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"reservation/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerReservation = await(httpResponseMessage.Content.ReadAsStringAsync());
                myReservation = JsonConvert.DeserializeObject<ReservationViewModel>(answerReservation);
            }
            return View(myReservation);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ReservationViewModel myReservation)
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
            
            return View();
        }

        public ActionResult CheckAvailability()
        {
            List<ReservationViewModel> reservations;
            return View();
        }
    }
}