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
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUserManager UserManager {
            get => _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();


            set => _userManager = value; }
        public ApplicationSignInManager SignInManager {
            get => _signInManager??HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            set => _signInManager = value; }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                 
                case SignInStatus.LockedOut:
                    return View("Lockout");
                  
                case SignInStatus.RequiresVerification:
                    
                case SignInStatus.Failure:
                    
                default:
                    ModelState.AddModelError("", "无效的登录尝试。");
                    return View(model);
            }
        }
        /// <summary>
        /// 用于判断是否为本地url的方法 Url.IsLocalUrl() 的注意事项：
        ///此方法是以判断传入的url字符串的开头是否为 "/" 为依据来判断是否为本地url，
        ///   所以如果传入的url字符串是完整路径(比如:http://xxx.xxx.com/abc)会返回false，
        /// 需要传入相对路径才会返回true(比如:/abc)
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

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
                  return  RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);   
            }
        }
    }
}