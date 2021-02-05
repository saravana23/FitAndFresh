using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemMenuController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ItemMenuController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var itemInMenu = await _db.ItemInMenu.Include(s=>s.Cat).ToListAsync();
            return View(itemInMenu);
            
        }
    }
}
