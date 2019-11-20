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
using System.IO;
using KnowledgeMVCSite.Util;

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
        
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,Title,Context")] Knowledge knowledge)
        {
            String[] fileNameArr =null;
            ViewBag.IsEdit =bool.Parse(Request["IsEdit"]);
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", knowledge.CategoryId);
            var accessories = Request["accessoHidden"];
            if (!string.IsNullOrEmpty(accessories))
            {
                fileNameArr =accessories.Split('|') ;
            }
            

            if (ModelState.IsValid)
            {

                ActionResult a = await Task.Run<ActionResult>(() =>
                {
                    try
                    {
                        if (ViewBag.IsEdit)
                        {
                            var know = db.Knowledges.Find(int.Parse(Request["Id"]));
                            know.CreateTime = DateTime.Now;
                            know.Context = knowledge.Context;
                            know.Title = knowledge.Title;
                            know.CategoryId = knowledge.CategoryId;
                            know.User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).Single();
                            db.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }
                        
                            var newKnowledge = new Knowledge
                            {
                                CreateTime = DateTime.Now,
                                Context = knowledge.Context,
                                Title = knowledge.Title,
                                CategoryId = knowledge.CategoryId,
                                User = db.Users.Where(p => p.Email == HttpContext.User.Identity.Name).Single(),


                            };




                        db.Knowledges.Add(newKnowledge);
                        db.SaveChanges();
                        if (fileNameArr != null)
                        {
                            foreach (var fileName in fileNameArr)
                            {
                                var file = db.Accessories.Where(p => p.FileName == fileName).Single();
                                file.Knowledge = newKnowledge;
                            }

                        }

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
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {

            var knowledge = await db.Knowledges.FindAsync(id);
            ViewBag.CategoryId = new SelectList(db.Categorys, "CategoryId", "Name", knowledge.CategoryId);
            ViewBag.Title = "编辑知识";
            ViewBag.IsEdit = true;
            return View("Create",knowledge);
        }
        [HttpGet]
        public async Task<Boolean> Delete(int id)

        {           
          
            var knowledge = await db.Knowledges.FindAsync(id);
            var acces = db.Accessories.Where(p => p.KnowledgeId==knowledge.Id).ToList();
            foreach (var item in acces)
            {
                db.Accessories.Remove(item);
                
            }
                    
                    db.Knowledges.Remove(knowledge);
                   int cout=  await db.SaveChangesAsync();
            if (cout !=0)
            {
                foreach (var item in acces)
                {
                    
                    var filePath = Server.MapPath("~/upfiles/" + item.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
      

        }

        
        [AllowAnonymous]
        public PartialViewResult DiscsussPartial(int knowledgeId)
        {
            ViewBag.knowledgeId = knowledgeId;
            var model = (from discuss in db.Discusses.Include("User") where discuss.Knowledge.Id == knowledgeId select discuss)  .ToList();

            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<PartialViewResult> Discuss([Bind(Include ="Context,KnowledgeId")] Discuss model)
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
            var model1 = (from discuss in db.Discusses.Include("User") where discuss.Knowledge.Id == model.KnowledgeId select discuss).ToList();

            return PartialView("DiscsussPartial", model1);

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
        [AllowAnonymous]
        public async Task<int> GetDiscussCout(int knowledgeId)
        {
            var kn = await db.Knowledges.FindAsync(knowledgeId);
            return kn.Discusses.Count;

        }
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public string GetCatalog()
        {

            var json = db.Categorys.ToList();
           return  Newtonsoft.Json.JsonConvert.SerializeObject(json);

        }
        [HttpPost]
        public async Task<string> GetFiles()
        {
            try
            {
               
                Dictionary<string, string> upfiles = new Dictionary<string, string>();
               
                var files = Request.Files;
                foreach (string item in files)
                {
                    HttpPostedFileBase file = files[item];
                    if (file != null)
                    {
                        //var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid()+Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/upfiles/") + fileName);
                        upfiles.Add(fileName, file.FileName);
                        db.Accessories.Add(new Accessory
                        {
                            CreateTime = DateTime.Now,
                            FileName = fileName,
                            OrginFileName=file.FileName,
                            UserId = User.Identity.GetUserId()  
                        });
                    }
                }
                 await db.SaveChangesAsync();
                return Newtonsoft.Json.JsonConvert.SerializeObject(upfiles);
            }
            catch (Exception)
            {
                return "上传失败";
            } 
        }
        [AllowAnonymous]
        public FileResult ViewFile(string fileName)
        {
            var path = Server.MapPath("~/upfiles/" + fileName);
           
            return File(path, MimeMapping.GetMimeMapping(path));



        }
        public FileResult PreviewHtml(string url)
        {

          
            var htmlUrl = Server.MapPath("~/upfiles/")+ Path.GetFileNameWithoutExtension( url) + ".html";

            if (! System.IO.File.Exists(htmlUrl))
            {
               
                string extension = Path.GetExtension(url);
                string physicalPath = Server.MapPath("~/upfiles/" + url);
                PreviewTool previewTool = new PreviewTool();
                switch (extension.ToLower())
                {
                    case ".xls":
                    case ".xlsx":
                        htmlUrl = previewTool.PreviewExcel(physicalPath, Server);
                        break;
                    case ".doc":
                    case ".docx":
                        htmlUrl = previewTool.PreviewWord(physicalPath, Server);
                        break;
                    case ".txt":
                        htmlUrl = previewTool.PreviewTxt(physicalPath);
                        break;
                    case ".pdf":
                        htmlUrl = previewTool.PreviewPdf(physicalPath);
                        break;
                }
            }
          
            

   

            return File(htmlUrl, MimeMapping.GetMimeMapping(htmlUrl));




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