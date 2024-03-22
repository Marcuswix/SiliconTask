using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Infrastructure.Controllers
{
    public class CoursesController : Controller
    {
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
                using var http = new HttpClient();

                var response = await http.GetAsync("https://localhost:7117/api/Courses");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    if (json.StartsWith("["))
                    {
                        var list = JsonConvert.DeserializeObject<List<CourseEntity>>(json);
                        return View(list);
                    }
                    else
                    {
                        var course = JsonConvert.DeserializeObject<CourseEntity>(json);
                        var dataList = new List<CourseEntity>() { course };
                        return View(dataList);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "At the moment there are no courses in the database.";
                    return View();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Something went wrong";
                return View(); 
                    }


        }
    }
}
