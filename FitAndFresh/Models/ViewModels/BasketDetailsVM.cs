using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models.ViewModels
{
    public class BasketDetailsVM
    {                      
        public OrderProcessingInfo OrderProcessingInfo { get; set; }
        public List<Basket> BasketList { get; set; }

    }
}
