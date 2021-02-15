using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models.ViewModels
{
    public class HomePageMenuViewModel
    {
        public IEnumerable<ItemInMenu> ItemInMenu { get; set; }
        public IEnumerable<Category> ItemCategory { get; set; }

    }
}
