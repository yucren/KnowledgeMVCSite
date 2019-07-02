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

}