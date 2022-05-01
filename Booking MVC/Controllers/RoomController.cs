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
        // Set the API URL
        string apiURL = "https://localhost:44359/";
        #region Index
        /// <summary>
        ///  Returns the Index Reservation View
        /// </summary>
        public async Task<ActionResult>  Index()
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            // Create the variables
            List<RoomViewModel> rooms = new List<RoomViewModel>();
            string answerRooms = string.Empty;
            // Send the HTP request to get all rooms
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetAll");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                rooms = JsonConvert.DeserializeObject<List<RoomViewModel>>(answerRooms);                
            }
            // Returns values to the view
            return View(rooms);
        }
        #endregion

        #region Create View
        /// <summary>
        ///  Returns the Create Room View
        /// </summary>
        public ActionResult Create()
        {

            return View();
        }
        #endregion
        #region Create Post Form
        /// <summary>
        ///  Create Room By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create(RoomViewModel myRoom)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();

            // Send the HTP request to create a room
            var response = await httpClient.PostAsync(apiURL + $"room/Insert", new StringContent(
    JsonConvert.SerializeObject(myRoom), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            
            return View();
        }
        #endregion

        #region Edit View
        /// <summary>
        ///  Returns the Edit Room View
        /// </summary>
        public async Task<ActionResult> Edit(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            // Create the variables
            RoomViewModel myRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            // Send the HTP request to get an specific room
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await (httpResponseMessage.Content.ReadAsStringAsync());
                myRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }

            // Returns values to the view
            ViewBag.IdRoom = id;
            return View(myRoom);
        }
        #endregion

        #region Edit Post Form
        /// <summary>
        ///  Edit Room By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Edit(RoomViewModel myRoom)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();

            // Send the HTP request to update an specific room
            var response = await httpClient.PutAsync(apiURL + $"room/Update", new StringContent(
    JsonConvert.SerializeObject(myRoom), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            
            return View();
        }
        #endregion

        #region Detail View
        /// <summary>
        ///  Returns the Detail Room View
        /// </summary>
        public async Task<ActionResult> Detail(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            // Create the variables
            RoomViewModel detailRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            // Send the HTP request to get an specific room by IdRoom
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                detailRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }
            // Returns values to the view
            return View(detailRoom);
        }
        #endregion

        #region Delete View
        /// <summary>
        ///  Returns the Delete Room View
        /// </summary>
        public async Task<ActionResult> Delete(int id)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            RoomViewModel deleteRoom = new RoomViewModel();
            string answerRooms = string.Empty;

            // Send the HTP request to get an specific room by IdRoom
            httpResponseMessage = await httpClient.GetAsync(apiURL + $"room/GetById/{id}/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                answerRooms = await(httpResponseMessage.Content.ReadAsStringAsync());
                deleteRoom = JsonConvert.DeserializeObject<RoomViewModel>(answerRooms);
            }
            // Returns values to the view
            return View(deleteRoom);
        }
        #endregion

        #region Delete Form Post
        /// <summary>
        ///  Delete Room By Submitted Form by Post Method
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Delete(RoomViewModel myRoom)
        {
            // Create an httpClient to send the requests to the API
            HttpClient httpClient = new HttpClient();

            // Send the HTP request to delete an specific room
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
            
            return View();
        }
        #endregion
    }
}
