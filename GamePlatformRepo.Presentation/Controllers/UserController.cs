using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using GamePlatformRepo.Core.Models;
using GamePlatformRepo.InfraStructure.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GamePlatformRepo.Controllers
{
    public class UserController : Controller
    {
         private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly GamePlatformDbContext dbContext;
        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            GamePlatformDbContext dbContext
            )
            {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Registration() {
            var errorMessage = base.TempData["Error"];

            if(errorMessage != null) {
                base.ModelState.AddModelError("All", errorMessage.ToString()!);
            }

            return base.View();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Login() {
            var errorMessage = base.TempData["Error"];

            if(errorMessage != null) {
                base.ModelState.AddModelError("All", errorMessage.ToString()!);
            }

            return base.View();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult LoginView(string? returnUrl) {
            var errorMessage = base.TempData["Error"];

            if(string.IsNullOrWhiteSpace(returnUrl) == false) {
                base.ViewData["returnUrl"] = returnUrl;
            }

            if(errorMessage != null) {
                base.ModelState.AddModelError("All", errorMessage.ToString()!);
            }

            return base.View();
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Registration([FromForm] User newUser) {
            try {
                var user = new IdentityUser {
                    UserName = newUser.Name,
                    Email = newUser.Mail,
                };
                await this.userManager.CreateAsync(user, newUser.Password!);

                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                
            }
            catch(Exception) {
                base.TempData["Error"] = "Something went wrong...";
                return base.Redirect("User/Registration");
            }
            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();
            return base.Redirect("User/LoginView");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login([FromForm] LoginDto dto) {
            User user = new User{
                Name = dto.Name,
                Password = dto.Password
            };
            var foundUser = await this.dbContext.Users.Where(p=> user.Name == p.Name).FirstOrDefaultAsync();

            if(foundUser == null) {
                base.TempData["Error"] = "Incorrect login";
                return base.Redirect("User/LoginView");
            }

            if(foundUser.Password != user.Password) {
                base.TempData["Error"] = "Incorrect login or password";
                return base.Redirect("User/LoginView");
            }
            if(string.IsNullOrWhiteSpace(dto.ReturnUrl) == false) {
                return base.Redirect(dto.ReturnUrl);
            }

            return base.Redirect("Games");
        }

        [HttpGet]

        public async Task<ActionResult> Logout([FromForm] LoginDto dto) {
            await signInManager.SignOutAsync();
            return base.RedirectToAction(actionName: "/", controllerName: "Game");
        }       

        [Authorize]
        public IActionResult MyInfo()
        {
            var user = new User {
                Id = int.Parse(base.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value!),
                Name = base.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value!,
                Mail = base.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value!,
            };
            return base.View(user);
        }

    }
}