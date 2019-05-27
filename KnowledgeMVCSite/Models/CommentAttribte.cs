using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeMVCSite.Models
{
    /// <summary>
    /// 列注释特征
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CommentAttribute:Attribute
    {
        private string value;
        /// <summary>
        /// 列注释
        /// </summary>
        /// <param name="value">值</param>
        public CommentAttribute(string value)
        {
            this.value = value;
        }
       public string Value {get
            {
                return value;

            }
            private set {

            }

        }
    }
}