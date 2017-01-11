using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Beginner.Blog.Core;
using Beginner.Blog.Models;
using Beginner.Blog.Core.Extension;

namespace Beginner.Blog.Controllers
{
    public class ArticlesController : ApiController
    {
        private ObjectContext db = new ObjectContext();
        // GET: api/Articles
        public IEnumerable<Article> GetArticles()
        {
            List<Article> articles = new List<Article>();     
            foreach(Article item in db.Articles)
            {
                Article article = new Article();
                article.Id = item.Id;
                article.Title = item.Title;
                article.Video = item.Video;
                article.ImageUrl = item.ImageUrl;
                article.Tags = item.Tags;
                article.Content = item.Content;
                article.CreateTime = item.CreateTime;
                article.Author = item.Author;
                articles.Add(article);
            }
            return articles;
            //return articles;
        }
        [HttpGet]
        public PagedList<Article> SearchArticles(string q ,int start)
        {
            List<Article> articles = new List<Article>();
            var art = db.Articles;
            return art.Where(w => w.Title.Contains(q)).ToPagedList(start,10);

            foreach (Article item in db.Articles)
            {
                Article article = new Article();
                article.Id = item.Id;
                article.Title = item.Title;
                article.Video = item.Video;
                article.ImageUrl = item.ImageUrl;
                article.Tags = item.Tags;
                article.Content = item.Content;
                article.CreateTime = item.CreateTime;
                article.Author = item.Author;
                articles.Add(article);
            }
           
            //return articles;
        }


        // GET: api/Articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult GetArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutArticle(int id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.Id)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Articles
        [ResponseType(typeof(Article))]
        public IHttpActionResult PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(article);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult DeleteArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.Id == id) > 0;
        }


        
    }
}