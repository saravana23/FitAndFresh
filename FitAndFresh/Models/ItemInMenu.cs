using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class ItemInMenu
    {
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string ProteinContent { get; set; }

        public double ItemPrice { get; set; }

        public string ItemPicture { get; set; }

        [Display(Name = "Category")]
        public int CatId { get; set; }

        [ForeignKey("CatId")]
        public virtual Category Cat { get; set; }

    }
}
