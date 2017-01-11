using Beginner.Blog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Beginner.Blog.Core;
using Beginner.Blog.Models;
using Beginner.Blog.Core.Extension;

namespace Beginner.Blog.Controllers
{
    public class SearchArticleController : ApiController
    {
        private ObjectContext db = new ObjectContext();
        // GET: api/SearchArticle
        public IEnumerable<Article> Get(string q, int start)
        {
            if (start < 1) { start = 1; }
            List<Article> articles = new List<Article>();
            var art = db.Articles.Where(w => w.Title.Contains(q)).ToPagedList(start, 10);            
            foreach (Article item in art.Items)
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
                article.VideoSrc = item.VideoSrc;
                articles.Add(article);
            }
            return articles;
        }

        // GET: api/SearchArticle/5
        public IHttpActionResult Get(int id)
        {
            var roles = db.Articles.Where(w => w.Id == id).Select(p => new
            {
                Id = p.Id,
                Title = p.Title,
                Video = p.Video,
                ImageUrl = p.ImageUrl,
                Tags = p.Tags,
                Content = p.Content,
                CreateTime = p.CreateTime,
                Author = p.Author,
                VideoSrc = p.VideoSrc
            });

            return Json(roles.First());

        }


    }
}
