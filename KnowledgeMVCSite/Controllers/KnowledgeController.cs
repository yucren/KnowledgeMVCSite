using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace KnowledgeMVCSite.Controllers
{
    public class KnowledgeController : Controller
    {

        KnowledgeModel db = new KnowledgeModel();
        // GET: Knowledge
        public ActionResult Index(string catalog)
        {
            ViewBag.Title = catalog;
            return View();
        }
        [Authorize]
        public ActionResult Create()
        {

            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]        
        [HttpPost]
        public  async Task<ActionResult>  Create(Knowledge knowledge)
        {
           
             
            if (ModelState.IsValid)
            {
               
                ActionResult a = await Task.Run<ActionResult>(() =>
                {
                    try
                    {



                        db.Knowledges.Add(new Knowledge
                        {

                            CreateTime = DateTime.Now,
                            Context = knowledge.Context,
                            Title = knowledge.Title,
                            User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).Single()

                        });
                         db.SaveChanges();
                    return    RedirectToAction("Index", "Home");

                    }
                    catch (Exception ex)
                    {

                        ModelState.AddModelError("", ex.Message);
                        return View(knowledge);
                    }
                    
                     
                }
                
               
                
                );
                return a;

            
            }
            //foreach (var item in ModelState.Values)
            //{
            //    foreach (var err in item.Errors)
            //    {
            //        ModelState.AddModelError("", err.ErrorMessage);
            //    }
            //}
           
            return View(knowledge);
        }




    }
}