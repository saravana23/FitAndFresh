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
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Display(Name = "Protein Content")]
        public string ProteinContent { get; set; }

        [Display(Name = "Price")]
        public double ItemPrice { get; set; }

        [Display(Name = "Picture")]
        public string ItemPicture { get; set; }

        [Display(Name = "Category")]
        public int CatId { get; set; }

        [ForeignKey("CatId")]
        public virtual Category Cat { get; set; }

    }
}
