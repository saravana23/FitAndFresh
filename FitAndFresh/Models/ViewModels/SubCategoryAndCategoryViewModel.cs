using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models.ViewModels
{
    public class SubCategoryAndCategoryViewModel
    {
        public IEnumerable<Category> ListOfCategories { get; set; }

        public SubCategory SubCategory { get; set; }

        public List<string> ListOfSubCategories { get; set; }

        public string Message { get; set; }
    }
}
