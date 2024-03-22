using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SiliconMVC.Controllers
{
    public class AdminCoursesController : Controller
    {
        private void SetDefaultViewValues()
        {
            ViewBag.ShowFooter = false;
            ViewBag.ShowChoices = false;
        }

        //Det ska gå att utföra CRUD på alla delar i Courses!!

        [HttpGet]
        [Route("/admin/courses")]
        public async Task<IActionResult> Index()
        {
            SetDefaultViewValues();

            using var http = new HttpClient();

            var response = await http.GetAsync("https://localhost:7117/api/Courses");

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<CourseEntity>>(json);

            return View(data);
        }
    }
}
