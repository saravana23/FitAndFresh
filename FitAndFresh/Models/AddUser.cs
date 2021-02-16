using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAndFresh.Models
{
    public class AddUser : IdentityUser
    {
        public string CustomerName { get; set; }
        public string AddressFirstLine { get; set; }

        public string AddressSecondLine { get; set; }

        public string PostCode { get; set; }


    }
}
