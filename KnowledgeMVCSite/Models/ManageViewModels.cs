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
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool BrowserRemembered { get; set; }
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