using System;
using System.Net;
using System.Web.Mvc;
using Articles.Models;
using Microsoft.AspNet.Identity;



namespace Articles.Controllers
{

    public class CommentsController : Controller
    {
        private ArticlesDBEntities db = new ArticlesDBEntities();

        // GET: Comments

        // GET: Comments/Create

        [Authorize]
        public ActionResult CreateComment(int? id)
        {
            Session["ArticleID"] = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        //POST: Comments/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "CommentName")] Comment comment)
        {
            string UserId = User.Identity.GetUserId();
            DateTime date = DateTime.Now;
            int Articleid = (int)Session["ArticleID"];
            Comment comment1 = new Comment()
            {
                ArticleId = Articleid,
                UserId = UserId,
                CommentDate = date,
                CommentName = comment.CommentName
            };
            db.Comments.Add(comment1);
            db.SaveChanges();
            return RedirectToAction("Index", "Articles");
        }
        //GET: Comments/Edit/5


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
