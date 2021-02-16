using System;
using System.Collections.Generic;
using System.Text;
using FitAndFresh.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitAndFresh.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<ItemInMenu> ItemInMenu { get; set; }

        public DbSet<AddUser> AddUser { get; set; }
    }
}
