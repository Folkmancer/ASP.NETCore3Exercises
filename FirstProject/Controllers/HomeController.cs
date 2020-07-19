using Microsoft.AspNetCore.Mvc;
using System;

namespace FirstProject.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var hour = DateTime.Now.Hour;
            var viewModel = hour < 12 ? "Good Morning" : "Good Afternoon";
            
            return View("MyView", viewModel);
        }
    }
}