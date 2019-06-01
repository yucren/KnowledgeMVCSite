using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

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