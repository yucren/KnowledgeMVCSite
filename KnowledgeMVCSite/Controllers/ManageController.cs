using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KnowledgeMVCSite.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        KnowledgeModel db = KnowledgeModel.Create();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manager
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "已更改您的密码"
            : message == ManageMessageId.SetPassowrdSuccess ? "已设置您的密码"
            : message == ManageMessageId.Error ? "出现错误"
            : message == ManageMessageId.AddPhoneSuccess ? "已添加你的电话号码。"
            : message == ManageMessageId.RemoveLoginSuccess ? "已删除你的电话号码。"
            : "";
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)

            };
            return View(model);
        }
        public ActionResult RoleMangar()
        {
            var roles = db.Roles.ToList();

            return View(roles);   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleMangar( string roleName)
        {
            db.Roles.Add(new IdentityRole(roleName));
            await db.SaveChangesAsync();
            var roles = db.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public PartialViewResult _RoleUserPartial(string id)
        {
            ViewBag.RoleId = id;
            var users = db.Roles.Find(id).Users.ToList();
            var userRoles= db.Roles.Find(id).Users;
            var userList = new List<ApplicationUser>();
            foreach (var item in userRoles)
            {
               userList.Add(db.Users.Find(item.UserId));
            }
            return PartialView(userList);
           

        }
        [HttpGet]       
        public string  UserList(string roleId)
        {
            var userList = new List<string>();
            var users = db.Roles.Find(roleId).Users;
            foreach (var item in users)
            {
                userList.Add(db.Users.Where(u => u.Id == item.UserId).Single().Id);
            }
            var json= db.Users.Where(p => userList.Contains(p.Id)==false).Select(x=> new
            {
                x.Id,
                x.Email,
                x.UserName

            });  
            return Newtonsoft.Json.JsonConvert.SerializeObject(json);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result == IdentityResult.Success)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
               

            }

            AddErrors(result);
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user !=null)
            {
                return user.PasswordHash != null;
            }
            return false;   
        }
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetPassowrdSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
    }
}