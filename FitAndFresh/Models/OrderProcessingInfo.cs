using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class OrderProcessingInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AddUser AddUser { get; set; }
        
        [Required]
        public DateTime DateOfOrder { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        public DateTime CollectionTime { get; set; }

        [Required]
        [NotMapped]
        public DateTime CollectionDate { get; set; }

        [Required]        
        [Display(Name = "Collection Name")]
        public string CollectionName { get; set; }

        [Required]        
        [Display(Name = "Telephone Number")]
        public string PhoneNumber { get; set; }

        public string OrderNumber { get; set; }

    }
}
