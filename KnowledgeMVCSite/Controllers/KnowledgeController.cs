using KnowledgeMVCSite.App_Start;
using KnowledgeMVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using KnowledgeMVCSite.Filter;
using Microsoft.AspNet.Identity;

namespace KnowledgeMVCSite.Controllers
{
    [ValidateInput(false)]
    [Authorize]
    public class KnowledgeController : Controller
    {

      public static KnowledgeModel db = new KnowledgeModel();
        
        // GET: Knowledge
        public ActionResult Index(string catalog)
        {
            ViewBag.Title = catalog;
            return View();
        }
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", 2);
            ViewBag.Title = "创建知识";
            ViewBag.IsEdit = false;
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public async Task<ActionResult> Create([Bind(Include = "Id,CategoryId,Title,Context")] Knowledge knowledge)
        {
            ViewBag.IsEdit =bool.Parse(Request["IsEdit"]);
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", knowledge.CategoryId);

            if (ModelState.IsValid)
            {

                ActionResult a = await Task.Run<ActionResult>(() =>
                {
                    try
                    {
                        if (ViewBag.IsEdit)
                        {
                            var know = db.Knowledges.Find(knowledge.Id);
                            know.CreateTime = DateTime.Now;
                            know.Context = knowledge.Context;
                            know.Title = knowledge.Title;
                            know.CategoryId = knowledge.CategoryId;
                            know.User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).Single();
                            db.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }

                        db.Knowledges.Add(new Knowledge
                        {
                            CreateTime = DateTime.Now,
                            Context = knowledge.Context,
                            Title = knowledge.Title,
                            CategoryId = knowledge.CategoryId,
                            User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).Single()

                        });
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");

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

        public async Task<ActionResult> Edit(int id)
        {

            var knowledge = await db.Knowledges.FindAsync(id);
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", knowledge.CategoryId);
            ViewBag.Title = "编辑知识";
            ViewBag.IsEdit = true;
            return View("Create",knowledge);


        }

        [NoCache]
        public PartialViewResult DiscsussPartial(int knowledgeId)
        {
            ViewBag.knowledgeId = knowledgeId;
            var model = (from discuss in db.Discusses.Include("User") where discuss.Knowledge.Id == knowledgeId select discuss)  .ToList();

            return PartialView(model);
        }

        [HttpPost]
        public async Task Discuss([Bind(Include ="Context,KnowledgeId")] Discuss model)
        {

            if (ModelState.IsValid)
            {
                Discuss discuss = new Discuss
                {
                    Context = model.Context,
                    CreateTime = DateTime.Now,
                    KnowledgeId=model.KnowledgeId,
                    User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).SingleOrDefault()
                };
                db.Discusses.Add(discuss);
                await db.SaveChangesAsync();
               
            }

        }

        [HttpPost]
        public PartialViewResult SearchPartial(string searchValue, int? pageCount, int? pageNum)
        {
            var model=  db.Knowledges.Include("User").Include("Category").Where(p => p.Title.Contains(searchValue) || p.Context.Contains(searchValue)).ToList();
            return PartialView(model);
        }


        public string  Praise(int knowlegeId)
        {
            var userid = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return "未登录";
            }

            if (db.Praises.Where(p=>p.KnowledgeId==knowlegeId && p.UserId== userid).Count()!=0)
            {
                db.Praises.Remove(db.Praises.Where(p => p.KnowledgeId == knowlegeId && p.UserId == userid).First());
                db.SaveChanges();
                return "点赞已取消";

            }
          var praise=  db.Praises.Add(new Models.Praise
            {
                  KnowledgeId =knowlegeId,
                   UserId= userid

          });

            if (praise !=null)
            {
                db.SaveChanges();
                return "点赞成功";
            }
            else
            {
                return "错误";
            }




        }
        [HttpGet]
        public async Task<int> GetDiscussCout(int knowledgeId)
        {
            var kn = await db.Knowledges.FindAsync(knowledgeId);
            return kn.Discusses.Count;

        }
        [HttpGet]
        public async Task<int> GetPraiseCout(int knowledgeId)
        {
            var kn = await db.Knowledges.FindAsync(knowledgeId);
            return kn.Praises.Count;

        }

        public PartialViewResult _ReportKnowPartial()
        {
            var userid = User.Identity.GetUserId();
            var kn = db.Knowledges.Where(p => p.User.Id == userid).ToList();
            return PartialView(kn);
        }

        public string GetCatalog()
        {

            var json = db.Categorys.ToList();
           return  Newtonsoft.Json.JsonConvert.SerializeObject(json);

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