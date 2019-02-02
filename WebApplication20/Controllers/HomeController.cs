using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApplication20.Models;
using WebApplication20.RestModels;

namespace WebApplication20.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Cookie
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                IsEssential = true, //<- there
                Expires = DateTime.Now.AddMonths(1),
            };
            Response.Cookies.Append("Name", "Stephan", cookieOptions);

            // Session
            HttpContext.Session.SetString("PlayerName", "Player 2");

            return View();
        }

        public IActionResult Privacy(string message, [FromForm] string secretMessage)
        {
            // Cookie
            var nameFromCookie = Request.Cookies["Name"];

            // Med ViewBag
            //ViewBag.Username = nameFromCookie;
            //ViewBag.Playername = HttpContext.Session.GetString("PlayerName");
            //ViewBag.Message = message;
            //ViewBag.SecretMessage = secretMessage;
            //return View();

            // Med model
            PrivacyMessage model = new PrivacyMessage()
            {
                Username = nameFromCookie,
                Playername = HttpContext.Session.GetString("PlayerName"),
                Message = message,
                SecretMessage = secretMessage
            };
            return View(model);
        }

        public IActionResult StartFiaSpel(int InputNumber)
        {
            var client = new RestClient("http://localhost:8998"); //domain + port number

            var request = new RestRequest("ludo/{id}", Method.GET);
            request.AddUrlSegment("id", InputNumber); // replaces matching token in request.Resource
            IRestResponse<LudoExample> ludoGameResponse = client.Execute<LudoExample>(request);
            var gameName = ludoGameResponse.Data.GameName;


            var newresult = InputNumber + 50;
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
