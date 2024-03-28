using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Infrastructure.Controllers
{
    public class CoursesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CoursesController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private void setValues()
        {
            ViewBag.ShowFooter = true;
            ViewBag.ShowChoices = false;
        }

        [HttpGet]
        [Route("/courses")]
        public async Task<IActionResult> Index()
        {
            setValues();

            try
            {
                if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
                {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await _httpClient.GetAsync($"https://localhost:7117/api/Courses?key={_configuration["ApiKey:Secret"]}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        if (json.StartsWith("{") || json.StartsWith("["))
                        {
                            var list = JsonConvert.DeserializeObject<List<CourseEntity>>(json);
                            return View(list);
                        }
                        else
                        {
                            var course = JsonConvert.DeserializeObject<CourseEntity>(json);
                            var dataList = new List<CourseEntity>() { course! };
                            return View(dataList);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "At the moment there are no courses in the database.";
                        return View();
                    }

                }
                else
                { return View(); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Something went wrong";
                return View();
            }




        }
    }
}
