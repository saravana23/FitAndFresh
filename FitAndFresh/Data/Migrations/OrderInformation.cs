using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class OrderInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderProcessingId { get; set; }

        [ForeignKey("OrderProcessingId")]
        public virtual OrderProcessingInfo OrderProcessingInfo { get; set; }


        [Required]
        public int ItemInMenuId { get; set; }

        [ForeignKey("ItemInMenuId")]
        public virtual ItemInMenu ItemInMenu { get; set; }

        [Required]
        public int Quantity { get; set; }
               
        public string ItemName { get; set; }
         
        [Required]
        public double ItemPrice { get; set; }

    }
}
