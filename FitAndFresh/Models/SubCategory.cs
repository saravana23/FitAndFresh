using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class SubCategory
    {
            [Key]
            public int Id { get; set; }

            [Display(Name = "SubCategory Name")] //this affects how the name property appears when we display this property in the UI
            [Required]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Category")]
            public int CategoryId { get; set; }

            [ForeignKey("CategoryId")] //this tells my application that the CategoryId in I set above on line 21, is a foreign key
            public virtual Category Category { get; set; } // I then create a public virtual called Category, which is of type Category, to tell my application which table is CategoryId (line 21 and line 23) a foreign table to
                                                            // virtual keyword is used to modify a method or property
                                                            // CategoryId is a foreign key, which represents the Category model 
                                                            // this virtual category object will be filled in using the include statement
            
    }
}

