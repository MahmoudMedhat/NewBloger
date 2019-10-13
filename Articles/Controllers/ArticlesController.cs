using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Articles.Models;
using Microsoft.AspNet.Identity;

namespace Articles.Controllers
{

    public class ArticlesController : Controller
    {
        private ArticlesDBEntities db = new ArticlesDBEntities();

        // GET: Articles
        [AllowAnonymous]
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.AspNetUser).Include(a => a.Category).Include(a => a.Comments);
            return View(articles.ToList());
        }

        // GET: Comments/Create

        //[Authorize]
        //public ActionResult CreateComment(int? id)
        //{
        //    Session["ArticleID"] = id;

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Article article = db.Articles.Find(id);
        //    if (article == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView("_CreateComment");
        //}

        ////POST: Comments/Create
        ////To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateComment([Bind(Include = "CommentName")] Comment comment)
        //{
        //    string UserId = User.Identity.GetUserId();
        //    DateTime date = DateTime.Now;

        //    int Articleid = (int)Session["ArticleID"];
        //    Comment comment1 = new Comment()
        //    {

        //        ArticleId = Articleid,
        //        UserId = UserId,
        //        CommentDate = date,
        //        CommentName = comment.CommentName
        //    };
        //    db.Comments.Add(comment1);
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "Articles");
        //}

        public ActionResult Filteration(string Name)
        {

            if (!String.IsNullOrEmpty(Name))
            {
                /* List<Article>*/
                List<Article> articles = db.Articles.Include(a => a.Category)
                .Where(x => x.Category.Name.Contains(Name)).Include(a => a.AspNetUser).Include(a => a.Comments).ToList();
                return PartialView(articles);
            }

            return PartialView(db.Articles.Include(a => a.AspNetUser).Include(a => a.Category).Include(a => a.Comments));

        }


        [Authorize(Roles = "Admin")]
        // GET: Articles/Create
        public ActionResult Create()
        {

            ViewBag.CategoryId = new SelectList(db.Categories, "id", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                string Id = User.Identity.GetUserId();
                DateTime date = DateTime.Now;
                Article article1 = new Article()
                {
                    Name = article.Name,
                    UserId = Id,
                    CategoryId = article.CategoryId,
                    ArticleDate = date
                };
                db.Articles.Add(article1);
                db.SaveChanges();
                ViewBag.Published = "The Article are Published";
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", article.UserId);
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "Name", article.CategoryId);
            return View(article);
        }
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
