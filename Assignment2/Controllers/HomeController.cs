using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        private readonly SportsDbContext _context;

        public HomeController(SportsDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            //No need to modify this line as it will go to its corresponding View
            return View();
        }
    }
}
