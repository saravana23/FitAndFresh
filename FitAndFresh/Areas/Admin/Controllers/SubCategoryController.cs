using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using FitAndFresh.Models.ViewModels;
using FitAndFresh.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ManagementAccount)]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        [TempData]
        public string ErrorMessage { get; set; }

        public SubCategoryController (ApplicationDbContext db)
        {
            _db = db;
        }
        

        //GET - Index
        public async Task<IActionResult> Index()
        {
            var subCategories = await _db.SubCategory.Include(s=>s.Category).ToListAsync();
            // Include() method is used as a way of performing eager loading
            //eager loading is a way to initialize an object  at the time of creation
            //so at the same time of creating the subCategories variable, I also use the include method to ensure that... 
            //here I am filling the category that is inside the subcategory object

            return View(subCategories);
        }

        //GET - Create
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                ListOfCategories = await _db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                ListOfSubCategories = await _db.SubCategory.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToListAsync()
                

            };
            return View(model);
            
            
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel sccmodel)
        {
            if (ModelState.IsValid)
            {
                var subCatExist = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == sccmodel.SubCategory.Name && s.Category.Id == sccmodel.SubCategory.CategoryId);
                    // this will retrieve all the records where the name matches with the same name in the model right here
                    //
                    if(subCatExist.Count() > 0)
                {
                    //This means that there is an error as there shouldn't be more than one
                    //return status message of some kind
                    ErrorMessage = "Error-There is already a subcategory called " +  subCatExist.First().Category.Name + ". Please choose a different name";
                    
                    
                }
                else
                {
                    _db.SubCategory.Add(sccmodel.SubCategory);
                    var i = sccmodel;
                    var j = sccmodel.SubCategory;

                    await _db.SaveChangesAsync();
                    // to save the subcat to the db
                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel sccVM = new SubCategoryAndCategoryViewModel() //create a new view model here
            {
                ListOfCategories = await _db.Category.ToListAsync(),
                SubCategory = sccmodel.SubCategory,
                ListOfSubCategories = await _db.SubCategory.OrderBy(s => s.Name).Select(s => s.Name).ToListAsync(),
                Message = ErrorMessage
                //These variable names on the left relate to the name fields in the SubCatAndCatViewModel

            };
            return View(sccVM);
        }



      
    }
}
