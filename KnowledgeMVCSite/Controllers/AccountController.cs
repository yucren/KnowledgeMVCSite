using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KnowledgeMVCSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager {
            get => _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();


            set => _userManager = value; }
        public ApplicationSignInManager SignInManager {
            get => _signInManager??HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            set => _signInManager = value; }




        // GET: Account
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.EMail,
                    Email = model.EMail
                };
                var result = await UserManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    RedirectToAction("Index", "Home");
                }
            }



        }
    }
}