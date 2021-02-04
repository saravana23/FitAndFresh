using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Category Name")] //this affects how the name property appears when we display this property in the UI
        [Required]
        public string Name { get; set; }
    }
}
