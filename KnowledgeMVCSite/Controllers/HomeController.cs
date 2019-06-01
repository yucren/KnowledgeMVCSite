using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeMVCSite.Filter;
using KnowledgeMVCSite.Models;

namespace KnowledgeMVCSite.Controllers
{
    
    public class HomeController : Controller
    {
        KnowledgeModel db = KnowledgeModel.Create();
        [NoCache]
        // GET: Home
        public ActionResult Index(int? catelog,int? pageCount,int? pageNum)
        {
            var pc = pageCount == null ? 1 : pageCount.Value;
            var pn = pageNum == null ? 10 : pageNum.Value;

            List<Knowledge> knowledges;
            if (catelog ==null)
            {
                knowledges = db.Knowledges.Include("Category").Include("User").
                Include("Praises").OrderByDescending(p=>p.CreateTime).Skip((pc-1) * pn).
                Take(pn).ToList();

            }
            else
            {
                knowledges = db.Knowledges.Include("Category").Include("User").Include("Praises").Where(c => c.CategoryId == catelog).Skip((pc - 1) * pn).
                    OrderByDescending(p => p.CreateTime).Take(pn).ToList();

            }

            return View(knowledges);
        }
      

        public ActionResult Help()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}