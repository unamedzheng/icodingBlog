using Beginner.Blog.Core;
using Beginner.Blog.Models;
using Beginner.Blog.Service.Interfaces;
using Beginner.Blog.Service.Messaging.Request;
using Beginner.Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beginner.Blog.Controllers
{
    public class ArticleController : BaseController
    {

        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IArticleService _articleService;
        public ArticleController(IRepository<Article> articleRepository, IRepository<Category> categoryRepository,
            IArticleService articleService)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _articleService = articleService;
        }
        public ActionResult List(int pageIndex = 1, int pageSize = 15)
        {
            //var query = _articleRepository.Table;
            var setting = GetSetting();
            if (setting != null)
                pageSize = setting.ManagePageSize;

            var request = new GetPageArticlesRequest(pageIndex, pageSize);
            #region Filter
            var title = Request["title"];
            var category = Request["category"];
            var state = Request["state"];
            int cId;
            int s;

            if (!string.IsNullOrEmpty(title))
            {
                request.Title = title;
                ViewBag.ArticleTitle = title;
            }
            if (int.TryParse(category, out cId) && cId != 0)
            {
                request.CategoryId = cId;
                ViewBag.CagegotyId = cId;
            }
            if (int.TryParse(state, out s) && s != 0)
            {
                request.Status = s;
                ViewBag.State = s;
            }

            //排序
            var field = Request["field"];
            var sort = Request["sort"];

            var createTimeSort = "desc";

            if (field != null && sort != null)
            {
                if (field == "CreateTime" && (sort == "asc" || sort == "desc"))
                {
                    request.Sort = new Sort("CreateTime", sort == "asc" ? SortMode.Asc : SortMode.Desc);
                    createTimeSort = sort == "asc" ? "desc" : "asc";
                }
            }
            ViewBag.CreateTimeSort = createTimeSort;
            #endregion

            var response = _articleService.GetPageArticles(request);

            ViewBag.Categories = _categoryRepository.FindAll();

            return View(response.Pages);
        }
        public ActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.FindAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ArticleModel model)
        {
            var article = new Article
            {
                Title = model.Title,
                Content = model.Content,
                Author = model.Author,
                CreateTime = DateTime.Now,
                IsTop = model.IsTop,
                State = model.State,
                Hits = model.Hits,
                Tags = model.Tags,
                CategoryId = model.CategoryId,
                Sort = model.Sort,
                MetaTitle = model.MetaTitle,
                MetaKeywords = model.MetaKeywords,
                MetaDescription = model.MetaDescription,
                VideoSrc = model.VideoSrc,
                Video=model.Video,
                FileUrl=model.FileUrl,
                ImageUrl = model.ImageUrl
                
            };

            _articleService.Insert(article);

            return RedirectToAction("List");
        }
        public ActionResult Edit(string articleId)
        {
            int id;
            if (int.TryParse(articleId, out id))
            {
                var article = _articleRepository.FindById(id);
                ViewBag.Categories = _categoryRepository.FindAll();
                if (article != null)
                    return View(article);
            }
            return RedirectToAction("NotFound", "Message");
        }

        [HttpPost]
        public ActionResult Delete()
        {
            var id = Request["id"];
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "删除失败，请刷新页面重试！" });
            int cId;
            if (!int.TryParse(id, out cId))
                return Json(new { success = false, message = "非法参数，请刷新页面重试！" });



            _articleRepository.Delete(p => p.Id == cId);

            return Json(new { success = true, message = "删除成功!" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ArticleModel model)
        {
            var article = _articleRepository.FindById(model.Id);

            article.Title = model.Title;
            article.Content = model.Content;
            article.Author = model.Author;
            article.IsTop = model.IsTop;
            article.State = model.State;
            article.Hits = model.Hits;
            article.Tags = model.Tags;
            article.CategoryId = model.CategoryId;
            article.Sort = model.Sort;
            article.MetaTitle = model.MetaTitle;
            article.MetaKeywords = model.MetaKeywords;
            article.MetaDescription = model.MetaDescription;
            article.ImageUrl = model.ImageUrl;
            article.Video = model.Video;
            article.VideoSrc = model.VideoSrc;
            article.FileUrl = model.FileUrl;
            _articleRepository.Update(article);

            return RedirectToAction("List");
        }
    }
}