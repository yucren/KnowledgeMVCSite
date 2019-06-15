namespace KnowledgeMVCSite.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.Cookies;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using KnowledgeMVCSite.Models;
    using System.Collections.Generic;

    public class KnowledgeModel : IdentityDbContext<ApplicationUser>
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
            Database.SetInitializer<KnowledgeModel>(new Initializer());
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

         public virtual DbSet<Knowledge> Knowledges { get; set; }
         public virtual DbSet<Category> Categorys { get; set;  }

         public virtual DbSet<Discuss> Discusses{ get; set; }

         public virtual DbSet<Praise> Praises { get; set; }

      

    }

    public class Praise
    {
        public Praise()
        {

            CreateTime = DateTime.Now;         


        }
        [Comment("ID")]
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        [Comment("创建时间")]
        public DateTime CreateTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("Knowledge")]
        public int KnowledgeId { get; set; }
        public Knowledge Knowledge { get; set; }

    }


    public class Discuss
    {
        public Discuss()
        {
           
        }

        [Comment("ID")]
        
        public int Id { get; set; }

        [Required(AllowEmptyStrings =false)]
        [Comment("评论")]
        [Display(Name = "评论")]
        [Index(IsClustered = false, IsUnique = false)]
        [StringLength(900)]
        public string Context { get; set; }

         [Comment("创建时间")]         
         [Display(Name ="评论时间")]
         [DataType(DataType.DateTime)]

        public DateTime CreateTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        [ForeignKey("Knowledge")]
        public int KnowledgeId { get; set; }

        public Knowledge Knowledge { get; set; }



    }

    public class Knowledge{
        [Required]
        [Comment("ID")]
        public int Id { get; set; }
        
        public ApplicationUser User { get; set; }
        
        [Comment("创建时间")]
        [DataType(DataType.DateTime)]         
        public DateTime CreateTime { get; set; }  
        public Category Category { get; set; }

        //[ForeignKey("category")]
        [Required]
        [Comment("知识类别")]
        [Display(Name ="知识类别")]
        public int CategoryId { get; set; }
        [Required]
        [Comment("标题")]
        [Index(IsClustered =false,IsUnique=false)]
        //[Column()]
        //[NotMapped]        
        [Display(Name = "标题")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "{0}标题不能大于{1}，不能小于{2}")]
        public string Title { get; set; }

        [Required]
        [Comment("正文")]
        [Display(Name = "描述知识详细内容")]
        [StringLength(maximumLength:2000000, MinimumLength= 10, ErrorMessage = "{0}标题不能大于{1}，不能小于{2}")]
        //[Index(IsClustered = false, IsUnique = false)]
        public virtual string Context { get; set; }
        public virtual ICollection<Praise> Praises { get; set; }
        public virtual ICollection<Discuss> Discusses { get; set; }

        



    }

    [Table("Categorys")]
    public class Category
    {
        [Comment("ID")]
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Comment("编码")]
        [Display(Name = "编码")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "{0}编码长度为{1}")]
        public string Code { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Comment("名称")]
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<Knowledge> Knowledges { get; set; }
    }

    public class ApplicationUser:IdentityUser
    {
        /// <summary>
        /// 城市 
        /// </summary>
       
        public virtual string City { get; set; }
        /// <summary>
        /// 
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

    /// <summary>
    /// 查找列注释
    /// SELECT ISNULL(b.value,'') AS 列注释, a.name FROM sys.columns a 
    /// LEFT JOIN sys.extended_properties b
    /// ON a.object_id =b.major_id AND a.column_id =b.minor_id
    /// WHERE a.object_id =OBJECT_ID('ScanInEntries')
    /// </summary>

    class Initializer:CreateDatabaseIfNotExists<KnowledgeModel>
    {
        /// <summary>
        /// 创建初始化种子
        /// </summary>
        /// <param name="context">数据库上下文</param>
        protected override void Seed(KnowledgeModel context)
        {
            
            var dbsets = context.GetType().GetProperties().Where(
                
                p => p.PropertyType.Name.Contains("DbSet`1"));


            foreach (var dbset in dbsets)
            {

                PropertyInfo[] properties = dbset.PropertyType.GetGenericArguments()[0].GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    CommentAttribute commentAttribute = propertyInfo.GetCustomAttribute<CommentAttribute>();
                    if (commentAttribute != null)
                    {

                        context.Database.ExecuteSqlCommand("EXECUTE sp_addextendedproperty N'MS_Description', '" + commentAttribute.Value + "', N'SCHEMA', N'dbo', N'TABLE', N'" + propertyInfo.DeclaringType.Name +"s"+ "', N'COLUMN', N'" + propertyInfo.Name + "'");

                    }

                }



            }




        }

    }
}