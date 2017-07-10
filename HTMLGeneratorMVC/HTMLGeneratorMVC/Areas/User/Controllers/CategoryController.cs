using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;
using PagedList;

namespace HTMLGeneratorMVC.Areas.User.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private GeneratorDb db = new GeneratorDb();
        private const int PageSize = 5;

        public ActionResult Index(string sortBy, int? page)
        {
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            var styles = db.Styles.Where(u => u.UserId == userId).AsQueryable();
            ViewBag.Count = styles.Count();

            ViewBag.Date = String.IsNullOrEmpty(sortBy) ? "CreatedStyle" : "";
            ViewBag.Title = String.IsNullOrEmpty(sortBy) ? "Title" : "";
            ViewBag.Category = sortBy == "Category" ? "CategoryDesc" : "Category";

            switch (sortBy)
            {
                case "CreatedStyle":
                    styles = styles.OrderBy(b => b.Time);
                    break;
                case "Category":
                    styles = styles.OrderBy(b => b.Category);
                    break;
                case "CategoryDesc":
                    styles = styles.OrderByDescending(b => b.Category);
                    break;
                case "Title":
                    styles = styles.OrderBy(b => b.Title);
                    break;
                case "TitleDesc":
                    styles = styles.OrderByDescending(b => b.Title);
                    break;
                default:
                    styles = styles.OrderByDescending(b => b.Time);
                    break;
            }

            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            return View(styles.ToPagedList(pageIndex, PageSize));
        }
        [HttpGet, ActionName("PartialIndex")]
        public ActionResult _PartialIndex(string searchBy, string sortBy, int? page, string message)
        {
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            var styles = db.Styles.Where(u => u.UserId == userId).AsQueryable();
            ViewBag.Count = styles.Count();
            styles = styles.Where(x => searchBy == null || searchBy == "" || x.Category.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.Title.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()));

           if(!string.IsNullOrEmpty(message))
           {
                ViewBag.Message = message;
           }

            ViewBag.Date = String.IsNullOrEmpty(sortBy) ? "CreatedStyle" : "";
            ViewBag.Title = String.IsNullOrEmpty(sortBy) ? "Title" : "";
            ViewBag.Category = sortBy == "Category" ? "CategoryDesc" : "Category";

            switch (sortBy)
            {
                case "CreatedStyle":
                    styles = styles.OrderBy(b => b.Time);
                    break;
                case "Category":
                    styles = styles.OrderBy(b => b.Category);
                    break;
                case "CategoryDesc":
                    styles = styles.OrderByDescending(b => b.Category);
                    break;
                case "Title":
                    styles = styles.OrderBy(b => b.Title);
                    break;
                case "TitleDesc":
                    styles = styles.OrderByDescending(b => b.Title);
                    break;
                default:
                    styles = styles.OrderByDescending(b => b.Time);
                    break;
            }


            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            return PartialView("_PartialIndex", styles.ToPagedList(pageIndex, PageSize));
        }

        public JsonResult GetTitleAndCategory(string term)
        {
            var searchTitleAndCategory = db.Styles.Where(x => x.Title.TrimStart().ToUpper().StartsWith(term.TrimStart().ToUpper()))
           .Select(s => s.Title).Distinct().ToList();
            var searchCategory = db.Styles.Where(x => x.Category.TrimStart().ToUpper().StartsWith(term.TrimStart().ToUpper()))
            .Select(s => s.Category).Distinct().ToList();
            searchTitleAndCategory = searchTitleAndCategory.Concat(searchCategory).ToList();
            return Json(searchTitleAndCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenameStyles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameStyles", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameStyles")]
        public ActionResult _PartialRenameStyles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameStyles", style);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameStyles([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameStyles", style);
        }

       
        [HttpPost, ActionName("DeleteStyles")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStylesConfirmed(int[] deleteInputs)
        {
            if (deleteInputs != null && deleteInputs.Length > 0)
            {
                for (int i = 0; i < deleteInputs.Length; i++)
                {
                    Style style = db.Styles.Find(deleteInputs[i]);
                    db.Styles.Remove(style);
                    db.SaveChanges();
                }
            }
            string message = null;
            if(deleteInputs.Length > 1)
            {
                message = "Styles was removed.";
            }
            else
            {
                message = "Style was removed.";
            }
            return RedirectToAction("PartialIndex", new { message = message });
        }

        public ActionResult RenameColors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameColors", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameColors")]
        public ActionResult _PartialRenameColors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameColors", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameColors([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameColors", style);
        }

        public ActionResult RenameTables(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameTables", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameTables")]
        public ActionResult _PartialRenameTables(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameTables", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameTables([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameTables", style);
        }

        public ActionResult RenameFormatting(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameFormatting", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameFormatting")]
        public ActionResult _PartialRenameFormatting(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameFormatting", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameFormatting([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameFormatting", style);
        }

        public ActionResult RenameButtons(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameButtons", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameButtons")]
        public ActionResult _PartialRenameButtons(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameButtons", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameButtons([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameButtons", style);
        }

     

        public ActionResult RenameLinks(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameLinks", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameLinks")]
        public ActionResult _PartialRenameLinks(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameLinks", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameLinks([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameLinks", style);
        }

 

        public ActionResult RenameImages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameImages", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameImages")]
        public ActionResult _PartialRenameImages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameImages", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameImages([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameImages", style);
        }


        public ActionResult RenameHeadings(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameHeadings", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameHeadings")]
        public ActionResult _PartialRenameHeadings(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameHeadings", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameHeadings([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameHeadings", style);
        }


        public ActionResult RenameParagraphs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameParagraphs", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameParagraphs")]
        public ActionResult _PartialRenameParagraphs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameParagraphs", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameParagraphs([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameParagraphs", style);
        }


        public ActionResult RenameUnorderedList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameUnorderedList", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameUnorderedList")]
        public ActionResult _PartialRenameUnorderedList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameUnorderedList", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameUnorderedList([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameUnorderedList", style);
        }

        public ActionResult RenameOrderedList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameOrderedList", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameOrderedList")]
        public ActionResult _PartialRenameOrderedList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameOrderedList", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameOrderedList([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameOrderedList", style);
        }

        public ActionResult RenameHeader(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameHeader", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameHeader")]
        public ActionResult _PartialRenameHeader(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameHeader", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameHeader([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameHeader", style);
        }


        public ActionResult RenameNavigation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameNavigation", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameNavigation")]
        public ActionResult _PartialRenameNavigation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameNavigation", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameNavigation([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameNavigation", style);
        }


        public ActionResult RenameMain(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameMain", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameMain")]
        public ActionResult _PartialRenameMain(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameMain", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameMain([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameMain", style);
        }

        public ActionResult RenameFooter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("RenameFooter", "Category", new { area = "Admin" });
            return View(style);
        }
        [HttpGet, ActionName("PartialRenameFooter")]
        public ActionResult _PartialRenameFooter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialRenameFooter", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameFooter([Bind(Include = "Id,Title,String1,String2,String3,String4,String5,String6,Time,UserId")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Attach(style);
                var entry = db.Entry(style);
                entry.State = EntityState.Modified;
                entry.Property(e => e.Category).IsModified = false;
                entry.Property(e => e.String1).IsModified = false;
                entry.Property(e => e.String2).IsModified = false;
                entry.Property(e => e.String3).IsModified = false;
                entry.Property(e => e.String4).IsModified = false;
                entry.Property(e => e.String5).IsModified = false;
                entry.Property(e => e.String6).IsModified = false;
                entry.Property(e => e.Time).IsModified = false;
                entry.Property(e => e.UserId).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("PartialIndex");
            }
            return PartialView("_PartialRenameFooter", style);
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
