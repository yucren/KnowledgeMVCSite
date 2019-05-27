using KnowledgeMVCSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace KnowledgeMVCSite.App_Start

{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.139.com");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential()
            {
                UserName = "18721600247@139.com",
                Password = "ycr111450"
            };
            
            MailMessage mailMessage = new MailMessage();
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = message.Subject;
            mailMessage.Sender = new MailAddress("18721600247@139.com", "yuchengren");
            //  mailMessage.ReplyTo = new MailAddress("18721600247@139.com");
            mailMessage.ReplyToList.Add(new MailAddress("18721600247@139.com"));
            mailMessage.ReplyToList.Add(new MailAddress("928184371@139.com"));
            mailMessage.To.Add(new MailAddress(message.Destination));
            mailMessage.From = new MailAddress("18721600247@139.com");
            smtpClient.SendCompleted += SmtpClient_SendCompleted;

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                return Task.FromResult(false);

            }
            // 在此处插入电子邮件服务可发送电子邮件。


            return Task.FromResult(0);
        }
        private void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            HttpResponse hr = e.UserState as HttpResponse;
            if (e.Error != null)
            {
                hr.Redirect("http://www.baidu.com");
            }
        }
    }
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {


        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {

        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.GetUserManager<KnowledgeModel>()));
            //配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            //配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                //RequireDigit = true,
                RequiredLength = 6,
                //RequireLowercase = true,
                //RequireNonLetterOrDigit = true,
                //RequireUppercase = true
            };
            //配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;//允许用户锁定
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);//5分钟锁定
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;//输错多少次以后锁定
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                //重置密码或者确认的令牌
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));

            }

            return manager;
        }
    }
    public class ApplicationSignInManager:SignInManager<ApplicationUser,string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager,IAuthenticationManager authenticationManager)
        :base(userManager,authenticationManager)
        {

        }

        public override  Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            ApplicationUser user= HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().Find(userName, password);

        

            if (user !=null   )
            {
                if (user.EmailConfirmed)
                {
                    return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);

                }
                else
                {
                    return Task.FromResult(SignInStatus.RequiresVerification);

                }



            }
          
            else
            {
                return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);

            }

        }
        /// <summary>
        /// 产生一种证件
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)(UserManager));

        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options ,IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);

        }


    }
}
    