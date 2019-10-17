using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        [AllowAnonymous]
        
        public ActionResult Login()
        {
            ViewBag.ReturnUrl = Request["ReturnUrl"];
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));           

            if (!ModelState.IsValid)
            {
                log.Error(model.Email + "登录失败");
                return View();
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
            switch (result)
            {
                case SignInStatus.Success:
                    log.Info(model.Email + "登录成功");
                    return RedirectToLocal(ReturnUrl);
                 
                case SignInStatus.LockedOut:
                    return View("Lockout");
                  
                case SignInStatus.RequiresVerification:
                    ModelState.AddModelError("", "请先验证邮箱。");
                    var user = await UserManager.FindAsync(model.Email, model.Password);
                    string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new RouteValueDictionary(new { UserId = user.Id, Code = code }), "http", HttpContext.Request.Url.Authority);
                    await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">这里</a>来确认你的帐户");
                    return RedirectToAction("DisplayEmail", "Account", new { result = "您的账号未通过邮件激活，请查收邮件", email = user.Email });
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
                    string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new RouteValueDictionary(new { UserId = user.Id, Code = code }), "https", HttpContext.Request.Url.Authority);
                    await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">这里</a>来确认你的帐户");
                    return RedirectToAction("DisplayEmail", "Account", new { result = "请查收邮件",email=user.Email });
                  //  await SignInManager.SignInAsync(user, false, false);
                  //return  RedirectToAction("Index", "Home");
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
        [AllowAnonymous]
        public ActionResult DisplayEmail(string result,string email)
        {
            ViewBag.result = result;
            ViewBag.email = email;

            return View();

        }
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string UserId ,string Code )
        {
            if (UserId==null || Code ==null)
            {
                return View("error");
            }
            var result = await UserManager.ConfirmEmailAsync(UserId, Code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");


        }


    }
}