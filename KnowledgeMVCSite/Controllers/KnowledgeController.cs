﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KnowledgeMVCSite.Controllers
{
    public class KnowledgeController : Controller
    {
        // GET: Knowledge
        public ActionResult Index(string catalog)
        {
            ViewBag.Title = catalog;
            return View();
        }


    }
}