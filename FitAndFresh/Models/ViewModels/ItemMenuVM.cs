using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models.ViewModels
{
    public class ItemMenuVM
    {

        public ItemInMenu ItemInMenu { get; set; }

        public IEnumerable<Category> Cat { get; set; }

    }
}
