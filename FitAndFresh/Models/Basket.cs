using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class Basket
    {
        public int Id { get; set; }

        public string BasketUserId { get; set; }


        [ForeignKey("BasketUserId")]
        public virtual AddUser AddUser { get; set; }
                
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual ItemInMenu ItemInMenu { get; set; }
                
        public int Quantity { get; set; }


    }
}
