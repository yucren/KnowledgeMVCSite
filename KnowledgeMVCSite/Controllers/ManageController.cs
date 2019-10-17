using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
        [HttpGet]
        public ActionResult MyKnowledge()
        {
            var model = new IndexViewModel();
            return View(model);
        }
        [Route("Manage/UserManage")]
        public ActionResult UserManage()
        {
            ViewBag.count = db.Users.Count();
            ViewBag.cur = 1;
            var userModel = db.Users.OrderBy(m => m.Email).Skip(0).Take(1).Select(m => new UserListViewModel
            {
                City = m.City,
                MailAddress = m.Email,
                PhoneNum = m.PhoneNumber,
                UserName = m.UserName
            }).ToList();

            return View(userModel);


        }
        [Route("Manage/UserManage/{pageLines}/{pageCur}")]
        public ActionResult UserManage(int pageLines, int pageCur)
        {
            ViewBag.count = db.Users.Count();
            ViewBag.cur = pageCur;
            var userModel = db.Users.OrderBy(m => m.Email).Skip((pageCur - 1)*pageLines).Take(pageLines).Select(m => new UserListViewModel
            {
                City = m.City,
                MailAddress = m.Email,
                PhoneNum = m.PhoneNumber,
                UserName = m.UserName

            }).ToList();

            return View(userModel);


        }

        [HttpGet]
        public ActionResult Setup()
        {
            EmailServerViewModel emailServer = new EmailServerViewModel();

            XmlDocument document = new XmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            document.Load(path + @"\config\mail.xml");
            emailServer.MailSendServer = document.SelectSingleNode("/mailServer/mailSendServer").InnerText;
            emailServer.MailSendUser = document.SelectSingleNode("/mailServer/mailSendUser").InnerText;
            emailServer.MailSendPassword = document.SelectSingleNode("/mailServer/mailSendPassword").InnerText;
            return View(emailServer);
        }
        [HttpPost]
        public string Setup(EmailServerViewModel model)
        {
            if (ModelState.IsValid)
            {
                XmlDocument document = new XmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                document.Load(path + @"\config\mail.xml");
                document.SelectSingleNode("/mailServer/mailSendServer").InnerText = model.MailSendServer;
                document.SelectSingleNode("/mailServer/mailSendUser").InnerText = model.MailSendUser;
                document.SelectSingleNode("/mailServer/mailSendPassword").InnerText = model.MailSendPassword;
                document.Save(path + @"\config\mail.xml");
                return "成功";
            }
            else
            {
                return "失败";
            }


        }



        public ActionResult RoleMangar()
        {
            var roles = db.Roles.ToList();

            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleMangar(string roleName)
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
            ViewBag.RoleName = db.Roles.Find(id).Name;
            var users = db.Roles.Find(id).Users.ToList();
            var userRoles = db.Roles.Find(id).Users;
            var userList = new List<ApplicationUser>();
            foreach (var item in userRoles)
            {
                userList.Add(db.Users.Find(item.UserId));
            }
            return PartialView(userList);

        }
        [HttpPost]
        public async Task<string> SaveUserToRole(string[] userIds, string roleName)
        {
            string errMsg = "";
            try
            {
                foreach (var userid in userIds)
                {
                    var result = await UserManager.AddToRoleAsync(userid, roleName);
                    if (!result.Succeeded)
                    {
                        errMsg += UserManager.FindById(userid).UserName + "添加角色失败/n";
                    }

                }
                if (string.IsNullOrEmpty(errMsg))
                {
                    return "添加成功";
                }
                else
                {
                    return errMsg;
                }
            }
            catch (Exception ex)
            {

                return "发生错误,可以存在部分用户添加角色成功，请刷新查看，错误如下：" + ex.Message;
            }





        }

        [HttpGet]
        public string UserList(string roleId)
        {
            var userList = new List<string>();
            var users = db.Roles.Find(roleId).Users;
            foreach (var item in users)
            {
                userList.Add(db.Users.Where(u => u.Id == item.UserId).Single().Id);
            }
            var json = db.Users.Where(p => userList.Contains(p.Id) == false).Select(x => new
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
            if (user != null)
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