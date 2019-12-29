using KnowledgeMVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;



namespace KnowledgeMVCSite.Controllers.Api
{
    public class KnowlegeController : ApiController
    {
        // GET: api/Knowlege
        KnowledgeModel db = KnowledgeModel.Create();
        
        // GET: Home      
        public string Get  ()
        {
           

          var Knowledges =   from kl in db.Knowledges
            join cl in db.Categorys
            on kl.Category.CategoryId equals cl.CategoryId into cc
            from cg in cc.DefaultIfEmpty()
            join user in db.Users
            on kl.User.Id equals user.Id into ch
            from ci in ch.DefaultIfEmpty()
            select new
            {
                KnowledgeId = kl.Id,
                kl.Title,
                Context= (kl.Context),
                PraisesCount = kl.Praises.Count,
                DiscussesCount = kl.Discusses.Count,
                kl.CreateTime,
                cg.Name,
                UserName = ci.UserName
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject( Knowledges);
            


        }

        // GET: api/Knowlege/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Knowlege
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Knowlege/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Knowlege/5
        public void Delete(int id)
        {
        }
    }
}
