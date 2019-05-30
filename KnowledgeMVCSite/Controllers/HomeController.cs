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
        public ActionResult Index(int? catelog)
        {
            List<Knowledge> knowledges;
            if (catelog ==null)
            {
                knowledges = db.Knowledges.Include("Category").Include("User").ToList();

            }
            else
            {
                knowledges = db.Knowledges.Include("Category").Include("User").Where(c => c.CategoryId == catelog).ToList();

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