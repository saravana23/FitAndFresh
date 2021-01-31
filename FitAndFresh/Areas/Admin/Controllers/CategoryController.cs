using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        // this private readonly will be a local object
     
        

        public CategoryController(ApplicationDbContext db)
        // this db object is what we will retrieve from our container using dependency injection
        // so in this way we do not need to go to the database to start a connection
        // everything will be done through dependency injection
        {
            _db = db;
        }

        // GET - This is a get action method - it retrieves everything from the database and passes it into the view
        public async Task<IActionResult> Index()
        // public IActionResult Index
        // however .Net recommends that we use 'async' (above) and await (below)
        // this is usually recommended when we are performing multiple functionalities
        // but even though we are just performing one (return view db Category) we should still use async and await for consistency and to be more syntactically correct
        // all action methods should be made 'async', 'task' and 'await'
        {
            
            return View(await _db.Category.ToListAsync());
            // return View(_db.Category.ToList());
        }
    }
}
