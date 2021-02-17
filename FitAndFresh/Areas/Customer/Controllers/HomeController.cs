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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using FitAndFresh.Utility;

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

            var claimsI = (ClaimsIdentity)User.Identity;
            var claims = claimsI.FindFirst(ClaimTypes.NameIdentifier);

            if (claims!=null)
            {
                var counter = _db.Basket.Where(s => s.BasketUserId == claims.Value).ToList().Count();
                // the first part of this, returns where the basker user id is in the claims value
                // then performing Count on this, get's the total number of occurences of this
                // this essentially returns the number of items in the cart

                HttpContext.Session.SetInt32("SessionsBasketCounter", counter);
            }

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


        //GET - Details
        [Authorize]
        public async Task<IActionResult> Details(int id) 
        {
            var itemInMenuDb = await _db.ItemInMenu.Include(s => s.Cat).Where(i => i.Id == id).FirstOrDefaultAsync();

            Basket basket = new Basket()
            {
                ItemInMenu = itemInMenuDb,
                ProductId = itemInMenuDb.Id,

            };

            return View(basket);


        }

        //POST 
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Details(Basket basket)
        {
            var check = basket.Id;

            basket.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsI = (ClaimsIdentity)this.User.Identity;
                var claims = claimsI.FindFirst(ClaimTypes.NameIdentifier);
                basket.BasketUserId = claims.Value;


                Basket basketDb = await _db.Basket.Where(s => s.ProductId==basket.ProductId && s.BasketUserId == basket.BasketUserId).FirstOrDefaultAsync();

                if (basketDb == null)
                {
                    await _db.Basket.AddAsync(basket);
                }
                else
                {
                    basketDb.Quantity = basketDb.Quantity + basket.Quantity;
                }
                
                await _db.SaveChangesAsync();
                

                var basketCounter = _db.Basket.Where(s => s.BasketUserId == basket.BasketUserId).ToList().Count();
                HttpContext.Session.SetInt32("SessionsBasketCounter", basketCounter);

                return RedirectToAction("Index");

            }

            else
            {
                var itemInMenuDb = await _db.ItemInMenu.Include(s => s.Cat).Where(i => i.Id == basket.ProductId).FirstOrDefaultAsync();

                Basket basketObject = new Basket()
                {
                    ItemInMenu = itemInMenuDb,
                    ProductId = itemInMenuDb.Id,

                };

                return View(basketObject);
            }
            


        }


    }
}
