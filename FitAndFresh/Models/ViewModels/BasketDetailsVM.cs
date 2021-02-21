using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models.ViewModels
{
    public class BasketDetailsVM
    {                      
        public OrderProcessingInformation OrderProcessingInformation { get; set; }
        public List<Basket> BasketList { get; set; }

    }
}
