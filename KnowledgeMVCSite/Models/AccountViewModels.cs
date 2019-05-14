using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KnowledgeMVCSite.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("电子邮件")]
        public string EMail { get; set; }
        /// <summary>
        /// ASP.NET 动态数据启用数据模型中的 CRUD （创建、 读取、 更新和删除） 操作。
        /// 可以指定每个字段的字符的最小值和最大长度时禁止数据插入或更新。
        /// 对于字符数据类型MinimumLength和MaximumLength属性确定，
        /// 这样才能存储字符串所需的字节的最大数目。
        /// 可以使用复合格式设置错误消息中的占位符：{0}属性; 的名称{1}
        /// 的最大长度; 和{2}
        /// 的最小长度。 占位符对应于参数传递给String.Format方法在运行时。
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("密码")]       
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { set; get; }
        public string Code { get; set; }
    }
}