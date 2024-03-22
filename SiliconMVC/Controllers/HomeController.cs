using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Infrastructure.Services;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Controllers;

public class HomeController : Controller
{
    private readonly SubscribeServices _subscribeServices;

    public HomeController(SubscribeServices subscribeServices)
    {
        _subscribeServices = subscribeServices;
    }

    private void SetValues()
    {
        ViewBag.ShowFooter = true;
        ViewBag.ShowChoices = true;
    }

    [HttpGet]
    [Route("/")]
    public IActionResult Index()
    {
        SetValues();
        return View();
    }

    [HttpPost]
    public async Task <IActionResult> Subscribe(SubscribeModel model)
    {
        SetValues();
        
        if (ModelState.IsValid)
        {
            using var http = new HttpClient();

            var url = "https://localhost:7117/api/Subscribe";

            var json = JsonConvert.SerializeObject(model);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Your are now subscribing!";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong";
            }
        }

        return View("Index", model);
    }

}


//var result = await _subscribeServices.AddSubscriber(model);

//if(result.StatusCode == Infrastructure.Models.StatusCodes.OK)
//{
//    TempData["Message"] = "Your are now subscribing!";

//}
//else if(result.StatusCode == Infrastructure.Models.StatusCodes.EXISTS)
//{
//    TempData["ErrorMessage"] = "This email is already up for subscription.";
//}
//else
//{
//    TempData["ErrorMessage"] = "Something went wrong";
//}

//return View("Index", model);
