using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FitAndFresh.Data;
using FitAndFresh.Models;
using FitAndFresh.Models.ViewModels;
using FitAndFresh.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FitAndFresh.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BasketController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public BasketDetailsVM basketDetails { get; set; }
        public BasketController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            basketDetails = new BasketDetailsVM()
            {
                OrderProcessingInfo = new Models.OrderProcessingInfo()

            };

            basketDetails.OrderProcessingInfo.OrderTotal = 0;
            //Order total is initially set to 0

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //This will retrieve the details of the user that is logged in

            var basket = _db.Basket.Where(s => s.BasketUserId == claims.Value);
            //This will retrieve all the items that the user has in the basket, within the database

            if (basket != null)
            {
                basketDetails.BasketList = basket.ToList();

                //if basket is not null, then we populate BasketList (of the BasketDetailsVM) with the items within basket
            }

            foreach (var i in basketDetails.BasketList)
            {
                i.ItemInMenu = await _db.ItemInMenu.FirstOrDefaultAsync(s => s.Id == i.ProductId);
                basketDetails.OrderProcessingInfo.OrderTotal = basketDetails.OrderProcessingInfo.OrderTotal + (i.ItemInMenu.ItemPrice * i.Quantity);
                //this iterates through the items in BasketList and gets the total of the order
            }


            return View(basketDetails);
        }


        public async Task<IActionResult> Increase(int basketId)
        {
            var basket = await _db.Basket.FirstOrDefaultAsync(s => s.Id == basketId);

            basket.Quantity += 1;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Decrease(int basketId)
        {
            var basket = await _db.Basket.FirstOrDefaultAsync(s => s.Id == basketId);

            if (basket.Quantity == 1)
            {
                _db.Basket.Remove(basket);
                await _db.SaveChangesAsync();

                var basketCounter = _db.Basket.Where(s => s.BasketUserId == basket.BasketUserId).ToList().Count;
                HttpContext.Session.SetInt32("SessionsBasketCounter", basketCounter);
            }
            else
            {
                basket.Quantity -= 1;

                await _db.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(int basketId)
        {
            var basket = await _db.Basket.FirstOrDefaultAsync(s => s.Id == basketId);


            _db.Basket.Remove(basket);
            await _db.SaveChangesAsync();

            var basketCounter = _db.Basket.Where(q => q.BasketUserId == basket.BasketUserId).ToList().Count;
            HttpContext.Session.SetInt32("SessionsBasketCounter", basketCounter);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> OrderSummary()
        {
            basketDetails = new BasketDetailsVM()
            {
                OrderProcessingInfo = new Models.OrderProcessingInfo()

            };

            basketDetails.OrderProcessingInfo.OrderTotal = 0;
            //Order total is initially set to 0

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            AddUser addUser = await _db.AddUser.Where(s => s.Id == claims.Value).FirstOrDefaultAsync();
            //This will retrieve the details of the user that is logged in

            var basket = _db.Basket.Where(s => s.BasketUserId == claims.Value);
            //This will retrieve all the items that the user has in the basket, within the database

            if (basket != null)
            {
                basketDetails.BasketList = basket.ToList();

                //if basket is not null, then we populate BasketList (of the BasketDetailsVM) with the items within basket
            }

            basketDetails.OrderProcessingInfo.CollectionName = addUser.CustomerName;
            basketDetails.OrderProcessingInfo.PhoneNumber = addUser.PhoneNumber;
            basketDetails.OrderProcessingInfo.CollectionTime = DateTime.Now;


            foreach (var i in basketDetails.BasketList)
            {
                i.ItemInMenu = await _db.ItemInMenu.FirstOrDefaultAsync(s => s.Id == i.ProductId);
                basketDetails.OrderProcessingInfo.OrderTotal = basketDetails.OrderProcessingInfo.OrderTotal + (i.ItemInMenu.ItemPrice * i.Quantity);
                //this iterates through the items in BasketList and gets the total of the order
            }


            return View(basketDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> OrderPost(string tokenStripe)
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            basketDetails.BasketList = await _db.Basket.Where(s => s.BasketUserId == claims.Value).ToListAsync();
            //This will retrieve all the items that the user has in the basket, within the database

            basketDetails.OrderProcessingInfo.StatusOfPayment = StaticDetails.PaymentPendingStatus;
            basketDetails.OrderProcessingInfo.DateOfOrder = DateTime.Now;
            basketDetails.OrderProcessingInfo.UserId = claims.Value;
            basketDetails.OrderProcessingInfo.Status = StaticDetails.PaymentPendingStatus;
            //basketDetails.OrderProcessingInfo.CollectionTime = Convert.ToDateTime(basketDetails.OrderProcessingInfo.CollectionDate.ToShortDateString() + " " + basketDetails.OrderProcessingInfo.CollectionTime.ToShortTimeString());
            basketDetails.OrderProcessingInfo.CollectionTime = basketDetails.OrderProcessingInfo.CollectionDate + basketDetails.OrderProcessingInfo.CollectionTime.TimeOfDay;

            List<OrderInfo> listOrderInfo = new List<OrderInfo>();
            _db.OrderProcessingInfo.Add(basketDetails.OrderProcessingInfo);
            await _db.SaveChangesAsync();

            basketDetails.OrderProcessingInfo.OrderTotal = 0;


            foreach (var i in basketDetails.BasketList)
            {
                i.ItemInMenu = await _db.ItemInMenu.FirstOrDefaultAsync(s => s.Id == i.ProductId);

                OrderInfo orderInfo = new OrderInfo
                {
                    ItemInMenuId = i.ProductId,
                    OrderProcessingId = basketDetails.OrderProcessingInfo.Id,
                    //ItemName = i.ItemInMenu.Name,
                    ItemPrice = i.ItemInMenu.ItemPrice,
                    Quantity = i.Quantity
                };
                basketDetails.OrderProcessingInfo.OrderTotal += orderInfo.Quantity * orderInfo.ItemPrice;
                _db.OrderInfo.Add(orderInfo);
            }

            //await _db.SaveChangesAsync();

            _db.Basket.RemoveRange(basketDetails.BasketList);
            HttpContext.Session.SetInt32("SessionsBasketCounter", 0);

            await _db.SaveChangesAsync();

            var settings = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(basketDetails.OrderProcessingInfo.OrderTotal * 100),
                Currency = "gbp",
                Description = "Order Number: " + basketDetails.OrderProcessingInfo.Id,
                Source = tokenStripe,
                //ReceiptEmail = basketDetails.BasketList
                //I've set these fields as the minimum, but other fields can be added if needed - see ChargeCreateOptions strip model for other fields
            };

            var chargeService = new ChargeService();
            Charge chargeStripe = chargeService.Create(settings);

            if (chargeStripe.BalanceTransactionId == null)
            {
                basketDetails.OrderProcessingInfo.StatusOfPayment = StaticDetails.PaymentDeclinedStatus;
            }
            else
            {
                basketDetails.OrderProcessingInfo.OrderNumber = chargeStripe.BalanceTransactionId;
            }

            if (chargeStripe.Status.ToLower() == "succeeded")
            {
                basketDetails.OrderProcessingInfo.Status = StaticDetails.SubmittedStatus;
                basketDetails.OrderProcessingInfo.StatusOfPayment = StaticDetails.PaymentAcceptedStatus;
            }
            else
            {
                basketDetails.OrderProcessingInfo.StatusOfPayment = StaticDetails.PaymentDeclinedStatus;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

            //basketDetails.OrderProcessingInfo.OrderTotal = basketDetails.OrderProcessingInfo.OrderTotal + (i.ItemInMenu.ItemPrice * i.Quantity);
            //this iterates through the items in BasketList and gets the total of the order
        }
    }
}
    
