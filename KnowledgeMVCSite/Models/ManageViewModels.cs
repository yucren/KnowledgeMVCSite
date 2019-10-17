using KnowledgeMVCSite.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KnowledgeMVCSite.Models
{
    public class IndexViewModel
    {
        public class Know
        {
            public int Id { get; set; }
            public DateTime CreateTime { get; set; }
            public string Title { get; set; }


        }
       

        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool BrowserRemembered { get; set; }
        public List<Know> Knowledges {
            get
            {
                var userid =HttpContext.Current.User.Identity.GetUserId();
                var kn = KnowledgeController.db.Knowledges.Where(p => p.User.Id == userid).Select(
                    p=>new Know
                    {
                       Id= p.Id,
                       Title= p.Title,
                      CreateTime= p.CreateTime
                    }                   
                    
                    ).ToList();
                return kn;


            }

        }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="新密码")]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="确实新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。"   )]
        public string ConfirmPassword { get; set; }

    }
    public class EmailServerViewModel
    {
        [Required]
        [Display(Name = "邮件发送服务器")]
        public string MailSendServer { get; set; }
        [Required]
        [Display(Name = "邮箱发送用户名")]
        public string MailSendUser { get; set; }
        [Required]
        [Display(Name = "邮箱密码")]
        public string MailSendPassword { get; set; }

    }

    public class UserListViewModel
    {
        [Display(Name ="用户名")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱地址")]
        public string MailAddress { get; set; }
        [Display(Name = "创建日期")]
        [DataType( DataType.Date)]
        public DateTime CreateTime { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "电话号码")]
        public string PhoneNum { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }


    }

}