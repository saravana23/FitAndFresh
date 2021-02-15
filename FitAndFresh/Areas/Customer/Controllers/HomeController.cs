using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FitAndFresh.Models;
using FitAndFresh.Models.ViewModels;
using FitAndFresh.Data;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Controllers
{
    [Area("Customer")] /// By writing this, it lets my application know that the 'Home Controller' is inside the 'Customer' Area
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController (ApplicationDbContext db)
        {
            _db = db;
        }

       
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

 

        public async Task<IActionResult> Index()
        {
            HomePageMenuViewModel HomeVM = new HomePageMenuViewModel()
            {
                ItemInMenu = await _db.ItemInMenu.Include(s => s.Cat).ToListAsync(),
                ItemCategory = await _db.Category.ToListAsync(),
            };
            return View(HomeVM);
        }

        /// These methods are called "Actions in a controller" - correct terminology
        /// Every method inside of a controller is called an action
        /// An action method, usually has a corresponding view
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
