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
        public ActionResult Index()
        {
            var knowleges = db.Knowledges.ToList();           

            return View(knowleges);
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