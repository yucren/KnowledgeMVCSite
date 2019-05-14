namespace KnowledgeMVCSite.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.Cookies;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class KnowledgeModel :IdentityDbContext<ApplicationUser>
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“KnowledgeModel”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“KnowledgeMVCSite.Models.KnowledgeModel”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“KnowledgeModel”
        //连接字符串。
        /// <summary>
        /// 初始化上下文
        /// </summary>
        public KnowledgeModel()
            : base("name=KnowledgeModel", throwIfV1Schema: false)
        {
            
        }
        /// <summary>
        /// 创建上下文
        /// </summary>
        /// <returns>上下文</returns>

        public static KnowledgeModel Create()
        {
            return new KnowledgeModel();
        }
        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class ApplicationUser:IdentityUser
    {
        /// <summary>
        /// 城市 
        /// </summary>
        public virtual string City { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public virtual int Age { get; set; }

        /// <summary>
        /// 产生用户声明的标识
        /// 一组claims构成了一个identity，
        /// 具有这些claims的identity就是 ClaimsIdentity ，驾照就是一种ClaimsIdentity，
        /// 可以把ClaimsIdentity理解为“证件”，驾照是一种证件，护照也是一种证件。
        /// </summary>
        /// <param name="userManager">用户管理器</param>
        /// <returns>返回声明的标识</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }


    }
}