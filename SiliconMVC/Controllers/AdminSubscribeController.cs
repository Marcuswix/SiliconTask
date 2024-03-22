using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace SiliconMVC.Controllers
{
    [Authorize(Policy = "Admins")]
    public class AdminSubscribeController : Controller
    {
        private void SetDefaultViewValues()
        {
            ViewBag.ShowFooter = false;
            ViewBag.ShowChoices = false;
        }

        [HttpGet]
        [Route("/admin/subscribers")]
        public async Task<IActionResult> Index()
        {
            SetDefaultViewValues();

            using var http = new HttpClient();

            var response = await http.GetAsync("https://localhost:7117/api/Subscribe");

            if (response != null)
            {
                var json = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseModel>(json);

                if (apiResponse.StatusCode == 200)
                {
                    var data = apiResponse.ContentResult;
                    return View(data);
                }
                else
                {
                    TempData["ErrorMessage"] = "API request failed: " + apiResponse.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No response from API";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            SetDefaultViewValues();

            if (email != null)
            {
                using var http = new HttpClient();

                var response = await http.DeleteAsync($"https://localhost:7117/api/Subscribe?email={email}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "The email is off from subscription";
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "Not found: ";
                }
                else
                {
                    TempData["ErrorMessage"] = "No response from API";
                }
            }
            TempData["ErrorMessage"] = "Something went wrong";
            return RedirectToAction("Index");
        }


    }
}
