using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beginner.Blog.Core;
using Beginner.Blog.Models;
using Beginner.Blog.Core.Extension;
using Beginner.Blog.ViewModels;
using Beginner.Blog.Helper;


namespace Beginner.Blog.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly IRepository<Comment> _commentRepository;
        public CommentsController(IRepository<Comment> commentRepository)
        {
            _commentRepository  = commentRepository;
        }


        // GET: Comments
        public ActionResult Index()
        {
            var query = _commentRepository.FindAll();

            var list = query.ToList();

            return View(list);
        }
        public ActionResult List(int pageIndex = 1, int pageSize = 15)
        {
            var setting = GetSetting();
            if (setting != null)
                pageSize = setting.ManagePageSize;

            var query = _commentRepository.Table;

            var list = query.OrderByDescending(p => p.ArticleId)
                .ThenByDescending(p => p.CreateTime)
                .ToPagedList(pageIndex, pageSize, true);

            return View(list);
        }
        // GET: Comments/Details/5
       
     
              
        [HttpPost]
        public ActionResult Delete()
        {
            var id = Request["id"];
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "删除失败，请刷新页面重试！" });
            int cId;
            if (!int.TryParse(id, out cId))
                return Json(new { success = false, message = "非法参数，请刷新页面重试！" });

            

            _commentRepository.Delete(p => p.Id == cId);

            return Json(new { success = true, message = "删除成功!" });
        }

    }
}
