using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using FitAndFresh.Models;
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

        //GET - Create
        public IActionResult Create()
        {
            return View();
        }

       //POST - Create - For every post method, we must add [HttpPost] so application knows its a post method
       [HttpPost]       // For every post method, we must add[HttpPost] so application knows its a post method
       [ValidateAntiForgeryToken]       // To prevent cross-site-request-forgery attacks - this is only for Post methods
       public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid) // so that validation is done on the server side
            {
                //if valid
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); //nameof is used to prevent errors due to spelling mistakes that may occur
                // so I return back to index page after database has been updated
            }
            return View(category);
        }


        //GET - Edit - 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            //if id doesn't equal null
            var category = await _db.Category.FindAsync(id); //we will get category from database, using the id

            if (category == null) //if category cannot be found, we just return NotFound
            {
                return NotFound(); 
            }

            return View(category); // here we return the view for the category that was found using the id number

        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                // so we're passing the category object that we receive from the form and using it with the update method
                // this category object contains the id and name of the category
                // Update will search for the category item using the id (as this is the primary key)
                // then will update all of the other properties (however in this case, we only have one property to update ('Name')
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View(category); // if model state is invalid, we will return to the view page
        }

        //GET - Delete 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //if id doesn't equal null
            var category = await _db.Category.FindAsync(id); //we will get category from database, using the id

            if (category == null) //if category cannot be found, we just return NotFound
            {
                return NotFound();
            }

            return View(category); // here we return the view for the category that was found using the id number

        }


        //POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var category = await _db.Category.FindAsync(id);
            
            if (category == null)
            {
                return NotFound();
                //return View();
            }

            _db.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //GET - Detail
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        

    }
}
