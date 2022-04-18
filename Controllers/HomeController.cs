using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser>userManager) { 
        
        
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async   Task<IActionResult> Index()
        {
            var users = await  _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id) {
            var user = await _userManager.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
            return View(user);
        
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {

            var user = await _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result=await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }

            return View(user);

        }

        public IActionResult CreateRole() {

            return View();

        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleStore roleStore) {
            var roleExist = await _roleManager.RoleExistsAsync(roleStore.Role);
            if (!roleExist) {
                await _roleManager.CreateAsync(new IdentityRole(roleStore.Role));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role!=null)
            {
                RoleStore rs = new();
                rs.Id = role.Id;
                rs.Role = role.Name;

                return View(rs);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(RoleStore roleStore)
        {
            var role = await _roleManager.FindByIdAsync(roleStore.Id);
            if (role != null)
            {
                role.Name = roleStore.Role;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("IndexRole", "Home");
                }

                
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                RoleStore rs = new();
                rs.Id = role.Id;
                rs.Role = role.Name;

                return View(rs);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRole(RoleStore roleStore)
        {
            var role = await _roleManager.FindByIdAsync(roleStore.Id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRole", "Home");
                }


            }

            return View();
        }

        public async Task<IActionResult> IndexRole() {

            var roles = await _roleManager.Roles.ToListAsync();
            List<RoleStore> lst = new List<RoleStore>();
            foreach (var role in roles) {
                lst.Add(new RoleStore { Role=role.Name,Id=role.Id});
            }
            
            return View(lst);
        }
        



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
