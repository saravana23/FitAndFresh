using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAndFresh.Data;
using FitAndFresh.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ManagementAccount)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController (ApplicationDbContext db)
        {
            _db = db;
        }
     
        public async Task<IActionResult> Index()
        {
            var users = await _db.AddUser.ToListAsync();            
            
            return View(users);
        }
    }
}
