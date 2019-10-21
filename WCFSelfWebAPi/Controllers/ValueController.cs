using System;
using System.Web;
using System.Web.Http;

namespace WCFSelfWebAPi.Controllers
{
    public class ValueController : ApiController
    {
       public string Get(int id)
        {
            return "values" + id;
        }

    }
}
