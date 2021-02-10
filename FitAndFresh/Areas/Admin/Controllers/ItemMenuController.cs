using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using FitAndFresh.Models.ViewModels;
using FitAndFresh.Utility;
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
        
        [BindProperty]
        public ItemMenuVM IMenuVM { get; set; }
        //by using a bind property here, IMenuVM will be loaded by default 
        //as a parameter for each subsequent Method (Create(), Index(), CreateItemPost() etc)
        //so we no longer have to do: Create (ItemMenuVM IMenuVM)
        //as this will already be assumed because of the bind property

        public ItemMenuController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            IMenuVM = new ItemMenuVM()
            {
                Cat = _db.Category,
                ItemInMenu = new Models.ItemInMenu()
            };
        }

        public async Task<IActionResult> Index()
        {
            var itemInMenu = await _db.ItemInMenu.Include(s=>s.Cat).ToListAsync();
            return View(itemInMenu);
            
        }

        //GET - Create
        public IActionResult Create()
        {
            return View(IMenuVM);
        }


        //POST - Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItemPost()
        {
            if (!ModelState.IsValid)
            {
                return View(IMenuVM);
            };
            _db.ItemInMenu.Add(IMenuVM.ItemInMenu);
            await _db.SaveChangesAsync();

            //also need to add image saving

            string websitePath = _hostEnvironment.WebRootPath;
            var uploadedImage = HttpContext.Request.Form.Files;

            var itemFromDatabase = await _db.ItemInMenu.FindAsync(IMenuVM.ItemInMenu.Id);

            if(uploadedImage.Count > 0)
            {
                //image has been uploaded
                var upload = Path.Combine(websitePath, "images");
                //this combines the websiterootpath with the path of the images directory, to get the ultimate path

                var ext = Path.GetExtension(uploadedImage[0].FileName);
                //here I will get the extention of the file that was uploaded

                using (var fileStream = new FileStream(Path.Combine(upload, IMenuVM.ItemInMenu.Id + ext), FileMode.Create))
                {
                    uploadedImage[0].CopyTo(fileStream);
                }
                // So what above will do is it will copy the file to a location on the server and rename it


                //then after uploading the image, we need to make sure that inside the
                //database, we change that image column with the location of where the image is saved
                itemFromDatabase.ItemPicture = @"\images\" + IMenuVM.ItemInMenu.Id + ext;

            }
            else
            {
                //no file was uploaded, so use a default one
                var upload = Path.Combine(websitePath, @"images\" + StaticDetails.FoodIcon);
                System.IO.File.Copy(upload, websitePath + @"\images\" + IMenuVM.ItemInMenu.Id + ".png");
                // we will copy image from 'upload' to  websitepath\images\ItemInMenu.Id.png
                itemFromDatabase.ItemPicture = @"\images\" + IMenuVM.ItemInMenu.Id + "png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        //GET - Details
        public async Task<IActionResult> Details(int? id)
        {
            var item = await _db.ItemInMenu.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }


            return View(item);
        }


        //GET - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            
            if(id == null)
            {
                return NotFound();
            }

            IMenuVM.ItemInMenu =  _db.ItemInMenu.Include(s => s.Cat).SingleOrDefault(s => s.Id == id);


            if(IMenuVM.ItemInMenu == null)
            {
                return NotFound();
            }

            return View(IMenuVM);

            //var itemToDelete = _db.ItemInMenu.FindAsync(id);
            //var itemToDelete = _db.ItemInMenu.SingleOrDefault(s=>s.Id == id);
           // ItemMenuVM.ItemInMenu = await;

            //if (itemToDelete == null)
           // if(ItemMenuVM.ItemInMenu = null)
           // {
          //      return NotFound();
            //}
                        
           // return View(ItemMenuVM);



        }



        //POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var itemToDelete = await _db.ItemInMenu.FindAsync(id);

            var itemToDelete = _db.ItemInMenu.Include(s => s.Cat).SingleOrDefault(s => s.Id == id);


            if(itemToDelete == null)
            {
                return NotFound();
            }

            _db.Remove(itemToDelete);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        



}
}
