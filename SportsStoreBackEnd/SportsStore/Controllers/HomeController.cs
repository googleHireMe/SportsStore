using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private DataContext context;
        public HomeController(DataContext ctx)
        {
            context = ctx;
        }
        public IActionResult Index()
        {
            return Json("Hello world!");
        }
    }
}
