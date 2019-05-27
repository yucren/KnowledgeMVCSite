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
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name",2);

            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]        
        [HttpPost]
        public  async Task<ActionResult>  Create([Bind(Include = "CategoryId,Title,Context")] Knowledge knowledge)
        {
          
            var id = HttpContext.Request["CategoryId"];
           
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name",knowledge.CategoryId);

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
                            CategoryId = knowledge.CategoryId,
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
        
        //public IEnumerable<SelectListItem> GetCategory()
        //{
        //   var categories= from category in db.Categories
        //    select new SelectListItem
        //    {
        //        Value = category.Id.ToString(),
        //        Text = category.Name,
        //        Selected =false,
                
                

        //    };
        //    return categories.AsEnumerable<SelectListItem>();


        //}




    }
}