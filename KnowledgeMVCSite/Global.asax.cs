using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using KnowledgeMVCSite.App_Start;
using System.Web.WebPages;

namespace KnowledgeMVCSite
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Application["countOnline"] = 0;
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //注册全局过滤器
            //以下为全局授权过滤器            
            //  FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //自定义显示模式
            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("WinPhone")
            //{
            //    ContextCondition = (context) =>
            //    {
            //   return  context.GetOverriddenUserAgent().IndexOf("Windows phone OS", StringComparison.OrdinalIgnoreCase) >= 0;

            //    }


            //});
            //视图引擎配置
          //  ViewEngines.Engines.Clear();
            
        }
        void Session_Start(object sender, EventArgs e)
        {

            Application["countOnline"] = int.Parse(Application["countOnline"].ToString()) + 1;

        }
        void Session_End(object sender, EventArgs e)
        {
            Application["countOnline"] = int.Parse(Application["countOnline"].ToString()) - 1;


        }
    }
}