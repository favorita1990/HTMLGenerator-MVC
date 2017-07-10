using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace HTMLGeneratorMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private GeneratorDb db = new GeneratorDb();
        private const int PageSize = 10;
        private const int PageSizeStyle = 5;

        // GET: Admin/Administration
        public ActionResult Index(string sortBy, int? page)
        {
            ViewBag.CreatedTimeSortParm = String.IsNullOrEmpty(sortBy) ? "CreatedTime" : "";
            ViewBag.FirstNameSortParm = sortBy == "FirstName" ? "FirstNameDesc" : "FirstName";
            ViewBag.LastNameSortParm = sortBy == "LastName" ? "LastNameDesc" : "LastName";
            ViewBag.EmailSortParm = sortBy == "Email" ? "EmailDesc" : "Email";
            ViewBag.GenterSort = sortBy == "Genter" ? "GenterDesc" : "Genter";

            ViewBag.CountUsers = db.Users.Count() - 1;

            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var users = db.Users.AsQueryable();

            switch (sortBy)
            {
                case "CreatedTime":
                    users = users.OrderBy
                     (b => b.Time);
                    break;
                case "Genter":
                    users = users.OrderBy
                    (b => b.Genter);
                    break;
                case "GenterDesc":
                    users = users.OrderByDescending
                    (b => b.Genter);
                    break;
                case "LastName":
                    users = users.OrderBy
                    (b => b.LastName);
                    break;
                case "LastNameDesc":
                    users = users.OrderByDescending
                    (b => b.LastName);
                    break;
                case "Email":
                    users = users.OrderBy
                            (c => c.Email);
                    break;
                case "EmailDesc":
                    users = users.OrderByDescending
                    (c => c.Email);
                    break;
                case "FirstName":
                    users = users.OrderBy
                    (c => c.FirstName);
                    break;
                case "FirstNameDesc":
                    users = users.OrderByDescending
                    (c => c.FirstName);
                    break;
                default:  // Name ascending
                    users = users.OrderByDescending
                    (b => b.Time);
                    break;
            }

            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
            return View(users.ToPagedList(pageIndex, PageSize));
        }
        [ActionName("PartialIndex")]
        public ActionResult _PartialIndex(string searchBy, string sortBy, int? page, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CreatedTimeSortParm = String.IsNullOrEmpty(sortBy) ? "CreatedTime" : "";
            ViewBag.FirstNameSortParm = sortBy == "FirstName" ? "FirstNameDesc" : "FirstName";
            ViewBag.LastNameSortParm = sortBy == "LastName" ? "LastNameDesc" : "LastName";
            ViewBag.EmailSortParm = sortBy == "Email" ? "EmailDesc" : "Email";
            ViewBag.UserIdSortParm = sortBy == "Genter" ? "GenterDesc" : "Genter";

            ViewBag.CountUsers = db.Users.Count() - 1;

            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var users = db.Users.AsQueryable();

            users = users.Where(x => searchBy == null || searchBy == "" || x.FirstName.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.LastName.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.Genter.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.Email.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()));

            switch (sortBy)
            {
                case "CreatedTime":
                    users = users.OrderBy
                     (b => b.Time);
                    break;
                case "Genter":
                    users = users.OrderBy
                    (b => b.Genter);
                    break;
                case "GenterDesc":
                    users = users.OrderByDescending
                    (b => b.Genter);
                    break;
                case "LastName":
                    users = users.OrderBy
                    (b => b.LastName);
                    break;
                case "LastNameDesc":
                    users = users.OrderByDescending
                    (b => b.LastName);
                    break;
                case "Email":
                    users = users.OrderBy
                            (c => c.Email);
                    break;
                case "EmailDesc":
                    users = users.OrderByDescending
                    (c => c.Email);
                    break;
                case "FirstName":
                    users = users.OrderBy
                    (c => c.FirstName);
                    break;
                case "FirstNameDesc":
                    users = users.OrderByDescending
                    (c => c.FirstName);
                    break;
                default:  // Name ascending
                    users = users.OrderByDescending
                    (b => b.Time);
                    break;
            }

            return PartialView("_PartialIndex", users.ToPagedList(pageIndex, PageSize));
        }

        public JsonResult GetUsersStuff(string term)
        {
            var allUserStuff = db.Users.Where(x => x.FirstName.TrimStart().ToUpper().StartsWith(term.TrimStart().ToUpper()))
           .Select(s => s.FirstName).Distinct().ToList();
            var lastName = db.Users.Where(x => x.LastName.TrimStart().ToUpper().StartsWith(term.TrimStart().ToUpper()))
            .Select(s => s.LastName).Distinct().ToList();
            var email = db.Users.Where(x => x.Email.TrimStart().ToUpper().StartsWith(term.TrimStart().ToUpper()))
            .Select(s => s.Email).Distinct().ToList();
            lastName = lastName.Concat(email).ToList();
            allUserStuff = allUserStuff.Concat(lastName).ToList();
            return Json(allUserStuff, JsonRequestBehavior.AllowGet);
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

        // GET: Admin/Administration/Details/5
        public ActionResult Details(int? id, int? userId, string searchBy, string sortBy, int? page)
        {
            UserModel user = null;
            if (userId != null)
            {
                user = db.Users.Find(db.Styles.Where(x => x.Id == userId).FirstOrDefault().UserId);
            }
            else
            {
                user = db.Users.Find(id);
            }

            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = user.Id;
            ViewBag.UserEmail = user.Email;
            var styles = db.Styles.Where(u => u.UserId == user.Id).AsQueryable();
            ViewBag.Count = styles.Count();
            styles = styles.Where(x => searchBy == null || searchBy == "" || x.Category.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.Title.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()));

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

            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
            return View(styles.ToPagedList(pageIndex, PageSizeStyle));
        }
        [HttpGet, ActionName("PartialDetails")]
        public ActionResult _PartialDetails(int? id, int? userId, string searchBy, string sortBy, int? page, string message)
        {
            UserModel user = null;
            if(userId != null)
            {
                user = db.Users.Find(db.Styles.Where(x => x.Id == userId).FirstOrDefault().UserId);
            }
            else
            {
                user = db.Users.Find(id);
            }

            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = user.Id;
            ViewBag.UserEmail = user.Email;
            var styles = db.Styles.Where(u => u.UserId == user.Id).AsQueryable();
            ViewBag.Count = styles.Count();
            styles = styles.Where(x => searchBy == null || searchBy == "" || x.Category.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()) ||
           x.Title.TrimStart().ToUpper().StartsWith(searchBy.TrimStart().ToUpper()));

            if (!string.IsNullOrEmpty(message))
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


            return PartialView("_PartialDetails", styles.ToPagedList(pageIndex, PageSizeStyle));
        }

        [HttpPost, ActionName("DeleteStyles")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStylesConfirmed(int[] deleteInputs, int? id)
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
            if (deleteInputs.Length > 1)
            {
                message = "Styles was removed.";
            }
            else
            {
                message = "Style was removed.";
            }
            return RedirectToAction("PartialDetails", new { id = id, message = message });
        }

        // GET: Admin/Administration/Create
        public ActionResult Create()
        {
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
            return View();
        }
        [HttpGet, ActionName("PartialCreate")]
        public ActionResult _PartialCreate()
        {
            return PartialView("_PartialCreate");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,PasswordHash,UserName")] UserModel model, string genter)
        {
            if (!IsValidEmailAddress(model.Email))
            {
                TempData["emailValidation"] = "Please Enter Correct Email.";
                return PartialView("_PartialCreate", model);
            }
            if (model.Email == null)
            {
                TempData["email"] = "Please Enter Email.";
                return PartialView("_PartialCreate", model);
            }
            if (model.FirstName == null)
            {
                TempData["firstName"] = "Please Enter First Name.";
                return PartialView("_PartialCreate", model);
            }
            if (model.LastName == null)
            {
                TempData["lastName"] = "Please Enter Last Name.";
                return PartialView("_PartialCreate", model);
            }
            if ((genter != "1") && (genter != "0"))
            {
                TempData["genter"] = "Please Men!";
                return PartialView("_PartialCreate", model);
            }
            if (model.PasswordHash == null)
            {
                TempData["password"] = "Please Enter Password.";
                return PartialView("_PartialCreate", model);
            }
            var roleManager = new RoleManager<Identity2Role, int>(new RoleStore(db));
            var userManager = new UserManager<UserModel, int>(new UserStore(db));
            var PasswordHash = new PasswordHasher();
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExists("User"))
                {
                    roleManager.Create(new Identity2Role("User"));
                }
                model.Genter = genter;
                model.UserName = model.Email;
                model.PasswordHash = PasswordHash.HashPassword(model.PasswordHash);
                model.Time = DateTime.Now;
                userManager.Create(model);
                userManager.AddToRole(model.Id, "User");
                return RedirectToAction("PartialIndex", new { message = "User was created." });
            }

            return View(model);
        }

        // GET: Admin/Administration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = db.Users.Find(id);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
            return View(userModel);
        }
        // GET: Admin/Administration/Edit/5
        [HttpGet, ActionName("PartialEdit")]
        public ActionResult _PartialEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = db.Users.Find(id);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialEdit", userModel);
        }
        private static bool IsValidEmailAddress(string emailAddress)
        {
            return new System.ComponentModel.DataAnnotations
                                .EmailAddressAttribute()
                                .IsValid(emailAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PasswordHash,UserName")] UserModel model, string genter)
        {
            if (!IsValidEmailAddress(model.Email))
            {
                TempData["emailValidation"] = "Please Enter Correct Email.";
                return PartialView("_PartialEdit", model);
            }
            if (model.Email == null)
            {
                TempData["email"] = "Please Enter Email.";
                return PartialView("_PartialEdit", model);
            }
            if (model.FirstName == null)
            {
                TempData["firstName"] = "Please Enter First Name.";
                return PartialView("_PartialEdit", model);
            }
            if (model.LastName == null)
            {
                TempData["lastName"] = "Please Enter Last Name.";
                return PartialView("_PartialEdit", model);
            }
            if ((genter != "1") && (genter != "0"))
            {
                TempData["genter"] = "Please Men!";
                return PartialView("_PartialEdit", model);
            }
            if (model.PasswordHash == null)
            {
                TempData["password"] = "Please Enter Password.";
                return PartialView("_PartialEdit", model);
            }

            var PasswordHash = new PasswordHasher();
            if (ModelState.IsValid)
            {
                model.Time = DateTime.Now;
                model.Genter = genter;
                model.UserName = model.Email;
                model.PasswordHash = PasswordHash.HashPassword(model.PasswordHash);
                var entry = db.Entry(model);
                entry.State = EntityState.Modified;
                entry.Property(e => e.SecurityStamp).IsModified = false;
                entry.Property(e => e.EmailConfirmed).IsModified = false;
                entry.Property(e => e.LockoutEnabled).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("PartialIndex", new { message = "User was changed." });
            }
            return PartialView("_PartialEdit", model);
        }

        // POST: Admin/Administration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int[] deleteInputs)
        {
            if (deleteInputs != null && deleteInputs.Length > 0)
            {
                var UserManager = new UserManager<UserModel, int>(new UserStore(db));
                for (int i = 0; i < deleteInputs.Length; i++)
                {
                    UserModel user = db.Users.Find(deleteInputs[i]);
                    var stylesOfUser = db.Styles.Where(x => x.UserId == user.Id);

                    foreach (var item in stylesOfUser)
                    {
                        db.Styles.Remove(item);
                    }
                    await UserManager.RemoveFromRoleAsync(user.Id, "User");
                    await UserManager.DeleteAsync(user);

                    db.SaveChanges();
                }
            }

            string message = null;
            if (deleteInputs.Length > 1)
            {
                message = "Users was removed.";
            }
            else
            {
                message = "User was removed.";
            }
            return RedirectToAction("PartialIndex", new { message = message });
        }

        //Begin rename of all styles
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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
            ViewBag.UserId = style.UserId;
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
            }
            return PartialView("_PartialRenameStyles", style);
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
            }
            return View("_PartialRenameFormatting", style);
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Home", new { area = "User" });
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

                return RedirectToAction("PartialDetails", new { userId = style.Id, message = "Style was changed." });
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
