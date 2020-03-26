using KnowledgeMVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KnowledgeMVCSite.Controllers
{
    public class AskController : Controller
    {
        public static KnowledgeModel db = new KnowledgeModel();
        // GET: Ask
        public ActionResult Index()
        {
            ViewBag.Title = "提出新问题";

            return View();
        }
        [HttpGet]
        public ActionResult Ask()
        {
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", 2);
            ViewBag.Title = "提出新问题";
            ViewBag.IsEdit = false;
            return View();

        }
        [HttpPost]
        public ActionResult Ask(Problem problem)
        {
            if (ModelState.IsValid)
            {

            }
            return View();


        }
    }
}