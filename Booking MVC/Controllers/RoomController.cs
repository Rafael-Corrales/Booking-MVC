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
    public class RoomController : Controller
    {
        string apiURL = "https://localhost:44359/";
        public async Task<ActionResult>  Index()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);                
            }
            
            return View(rooms);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RoomViewModel myRoom)
        {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(apiURL + $"room/Insert", new StringContent(
    JsonConvert.SerializeObject(myRoom), Encoding.UTF8, "application/json"));
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
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                myRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }
            ViewBag.IdRoom = id;
            return View(myRoom);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoomViewModel myRoom)
        {

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PutAsync(apiURL + $"room/Update", new StringContent(
    JsonConvert.SerializeObject(myRoom), Encoding.UTF8, "application/json"));
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
            RoomViewModel detailRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                detailRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }
            return View(detailRoom);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel deleteRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                deleteRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }
            return View(deleteRoom);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(RoomViewModel myRoom)
        {
                HttpClient httpClient = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiURL + $"room/Delete"),
                    Content = new StringContent(JsonConvert.SerializeObject(myRoom), Encoding.UTF8, "application/json")
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

    }
}
