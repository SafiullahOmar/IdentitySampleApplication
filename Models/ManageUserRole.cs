using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class ManageUserRoles
    {

        public ApplicationUser AppUser { get; set; }
        public List<SelectListItem> roles { get; set; }

    }
}
