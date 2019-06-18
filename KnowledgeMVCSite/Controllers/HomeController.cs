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
        // GET: Home      
        public ActionResult Index(int? catalog, int? pageCount, int? pageNum, string searchValue)
        {
            ViewBag.catalog = catalog;
            ViewBag.searchValue = searchValue;
            var pc = pageCount == null ? 1 : pageCount.Value;
            var pn = pageNum == null ? 1 : pageNum.Value;
            var sv = searchValue ?? "";

            List<Knowledge> knowledges;

            if (catalog != null)
            {
            knowledges = db.Knowledges.Include("Category").Include("User").
            Include("Praises").Include("Discusses").Where(p => p.CategoryId == catalog && (p.Title.Contains(sv) || p.Context.Contains(sv))).
            OrderByDescending(p => p.CreateTime).Skip((pc - 1) * pn).
            Take(pn).ToList();
            }
            else
            {
                knowledges = db.Knowledges.Include("Category").Include("User").
                Include("Praises").Include("Discusses").Where(p => p.Title.Contains(sv) || p.Context.Contains(sv)).
                OrderByDescending(p => p.CreateTime).Skip((pc - 1) * pn).
                Take(pn).ToList();
            }
            if (pageCount != null && pageCount.Value > 1)
            {
                return PartialView("SearchPartial", knowledges);
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