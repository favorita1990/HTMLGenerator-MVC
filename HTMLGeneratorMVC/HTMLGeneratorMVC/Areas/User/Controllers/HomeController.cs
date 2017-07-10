using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;

namespace HTMLGeneratorMVC.Areas.User.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private GeneratorDb db = new GeneratorDb();
        public ActionResult Index(string message)
        {
            if(!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return View();
        }

        public ActionResult Styles(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.style = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }
            ViewBag.PickBodyColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"}
            };
            ViewBag.PickBodyTextSize = new List<SelectListItem>
            {
                new SelectListItem {Text = "100%", Value = "font-size:100%"},
                new SelectListItem {Text = "150%", Value = "font-size:150%"},
                new SelectListItem {Text = "200%", Value = "font-size:200%"},
                new SelectListItem {Text = "300%", Value = "font-size:300%"},
                new SelectListItem {Text = "400%", Value = "font-size:400%"}
            };
            ViewBag.HeadingTextColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };
            ViewBag.ParagraphTextColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountStyles = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Styles")).Count();
            if ((ViewBag.CountStyles > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Styles' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Styles", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialStylesIndex")]
        public ActionResult _PartialStylesIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.style = db.Styles.Find(id);
            }
            ViewBag.PickBodyColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"}
            };
            ViewBag.PickBodyTextSize = new List<SelectListItem>
            {
                new SelectListItem {Text = "100%", Value = "font-size:100%"},
                new SelectListItem {Text = "150%", Value = "font-size:150%"},
                new SelectListItem {Text = "200%", Value = "font-size:200%"},
                new SelectListItem {Text = "300%", Value = "font-size:300%"},
                new SelectListItem {Text = "400%", Value = "font-size:400%"}
            };
            ViewBag.HeadingTextColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };
            ViewBag.ParagraphTextColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountStyles = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Styles")).Count();
            if ((ViewBag.CountStyles > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Styles' to save!";
            return PartialView("_PartialStylesIndex");
        }

        [HttpPost, ActionName("PartialStyles")]
        public ActionResult _PartialStyles(string PickBodyColor, string PickBodyTextSize, string HeadingText, string HeadingTextColor,
            string ParagraphText, string ParagraphTextColor)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["PickBodyColor"] = PickBodyColor;
            TempData["PickBodyTextSize"] = PickBodyTextSize;
            TempData["HeadingText"] = HeadingText;
            TempData["HeadingTextColor"] = HeadingTextColor;
            TempData["ParagraphText"] = ParagraphText;
            TempData["ParagraphTextColor"] = ParagraphTextColor;
            return PartialView("_PartialStyles", style);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Styles(Style style)
        {
            if(string.IsNullOrEmpty(style.Title))
            {
               return PartialView("_PartialStyles");
            }
            if (Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            style.Category = "Styles";
            style.String1 = TempData["PickBodyColor"] as string;
            style.String2 = TempData["PickBodyTextSize"] as string;
            style.String3 = TempData["HeadingText"] as string;
            style.String4 = TempData["HeadingTextColor"] as string;
            style.String5 = TempData["ParagraphText"] as string;
            style.String6 = TempData["ParagraphTextColor"] as string;
            style.Time = DateTime.Now;
            style.UserId = userId;
            
                TempData["style"] = "Style was saved!";
                db.Styles.Add(style);
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Styles";
                styleOld.String1 = TempData["PickBodyColor"] as string;
                styleOld.String2 = TempData["PickBodyTextSize"] as string;
                styleOld.String3 = TempData["HeadingText"] as string;
                styleOld.String4 = TempData["HeadingTextColor"] as string;
                styleOld.String5 = TempData["ParagraphText"] as string;
                styleOld.String6 = TempData["ParagraphTextColor"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialStylesIndex");
        }

        public ActionResult Colors(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Color = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountColors = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Colors")).Count();
            if ((ViewBag.CountColors > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Colors' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Colors", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialColorsIndex")]
        public ActionResult _PartialColorsIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Color = db.Styles.Find(id);
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountColors = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Colors")).Count();
            if ((ViewBag.CountColors > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Colors' to save!";
            return PartialView("_PartialColorsIndex");
        }
        [HttpPost, ActionName("PartialColors")]
        public ActionResult _PartialColors(string color)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["color"] = color;
            return PartialView("_PartialColors", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Colors(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialColors");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Colors";
                style.String1 = TempData["color"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Colors";
                styleOld.String1 = TempData["color"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            
            return RedirectToAction("PartialColorsIndex");
        }

        public ActionResult Tables(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Table = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }
            ViewBag.BorderStyle = new List<SelectListItem>
            {
                new SelectListItem {Text = "solid", Value = "solid"},
                new SelectListItem {Text = "Dashed", Value = "dashed"},
                new SelectListItem {Text = "Dotted", Value = "dotted"},
                new SelectListItem {Text = "Double", Value = "double"},
                new SelectListItem {Text = "Groove", Value = "groove"},
                new SelectListItem {Text = "Ridge", Value = "ridge"},
                new SelectListItem {Text = "Inset", Value = "inset"},
                new SelectListItem {Text = "Outset", Value = "outset"}
            };
            ViewBag.DisplaySampleText = new List<SelectListItem>
            {
                new SelectListItem {Text = "Yes", Value = "Yes"},
                new SelectListItem {Text = "No", Value = "No"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountTables = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Tables")).Count();
            if ((ViewBag.CountTables > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Tables' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Tables", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialTablesIndex")]
        public ActionResult _PartialTablesIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Table = db.Styles.Find(id);
            }
            ViewBag.BorderStyle = new List<SelectListItem>
            {
                new SelectListItem {Text = "solid", Value = "solid"},
                new SelectListItem {Text = "Dashed", Value = "dashed"},
                new SelectListItem {Text = "Dotted", Value = "dotted"},
                new SelectListItem {Text = "Double", Value = "double"},
                new SelectListItem {Text = "Groove", Value = "groove"},
                new SelectListItem {Text = "Ridge", Value = "ridge"},
                new SelectListItem {Text = "Inset", Value = "inset"},
                new SelectListItem {Text = "Outset", Value = "outset"}
            };
            ViewBag.DisplaySampleText = new List<SelectListItem>
            {
                new SelectListItem {Text = "Yes", Value = "Yes"},
                new SelectListItem {Text = "No", Value = "No"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountTables = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Tables")).Count();
            if ((ViewBag.CountTables > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Tables' to save!";
            return PartialView("_PartialTablesIndex");
        }
        [HttpPost, ActionName("PartialTables")]
        public ActionResult _PartialTables(string RowCount, string ColumnCount, string DisplaySampleText,
        string BorderStyle)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["RowCount"] = RowCount;
            TempData["ColumnCount"] = ColumnCount;
            TempData["DisplaySampleText"] = DisplaySampleText;
            TempData["BorderStyle"] = BorderStyle;
            return PartialView("_PartialTables", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tables(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialTables");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Tables";
                style.String1 = TempData["RowCount"] as string;
                style.String2 = TempData["ColumnCount"] as string;
                style.String3 = TempData["DisplaySampleText"] as string;
                style.String4 = TempData["BorderStyle"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Tables";
                styleOld.String1 = TempData["RowCount"] as string;
                styleOld.String2 = TempData["ColumnCount"] as string;
                styleOld.String3 = TempData["DisplaySampleText"] as string;
                styleOld.String4 = TempData["BorderStyle"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialTablesIndex");
        }

        public ActionResult Formatting(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Formatting = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.Format = new List<SelectListItem>
            {
                new SelectListItem {Text = "Bold Text", Value = "b"},
                new SelectListItem {Text = "Important Text", Value = "strong"},
                new SelectListItem {Text = "Italic Text", Value = "i"},
                new SelectListItem {Text = "Emphasized Text", Value = "em"},
                new SelectListItem {Text = "Marked Text", Value = "mark"},
                new SelectListItem {Text = "Small Text", Value = "small"},
                new SelectListItem {Text = "Sbscript Text", Value = "sub"},
                new SelectListItem {Text = "Superscript Text", Value = "sup"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountFormatting = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Formatting")).Count();
            if ((ViewBag.CountFormatting > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Formatting' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Formatting", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialFormattingIndex")]
        public ActionResult _PartialFormattingIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Formatting = db.Styles.Find(id);
            }

            ViewBag.Format = new List<SelectListItem>
            {
                new SelectListItem {Text = "Bold Text", Value = "b"},
                new SelectListItem {Text = "Important Text", Value = "strong"},
                new SelectListItem {Text = "Italic Text", Value = "i"},
                new SelectListItem {Text = "Emphasized Text", Value = "em"},
                new SelectListItem {Text = "Marked Text", Value = "mark"},
                new SelectListItem {Text = "Small Text", Value = "small"},
                new SelectListItem {Text = "Sbscript Text", Value = "sub"},
                new SelectListItem {Text = "Superscript Text", Value = "sup"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountFormatting = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Formatting")).Count();
            if ((ViewBag.CountFormatting > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Formatting' to save!";
            return PartialView("_PartialFormattingIndex");
        }
        [HttpPost, ActionName("PartialFormatting")]
        public ActionResult _PartialFormatting(string FormatText, string Format)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["FormatText"] = FormatText;
            TempData["Format"] = Format;
            return PartialView("_PartialFormatting", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Formatting(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialFormatting");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Formatting";
                style.String1 = TempData["FormatText"] as string;
                style.String2 = TempData["Format"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Formatting";
                styleOld.String1 = TempData["FormatText"] as string;
                styleOld.String2 = TempData["Format"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialFormattingIndex");
        }


        public ActionResult Buttons(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Button = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.PickButtonType = new List<SelectListItem>
            {
                new SelectListItem {Text = "Button", Value = "button"},
                new SelectListItem {Text = "Submit", Value = "submit"},
                new SelectListItem {Text = "Reset", Value = "reset"}
            };
            ViewBag.ButtonTarget = new List<SelectListItem>
            {
                new SelectListItem {Text = "_blank", Value = "_blank"},
                new SelectListItem {Text = "_self", Value = "_self"},
                new SelectListItem {Text = "_parent", Value = "_parent" },
                new SelectListItem {Text = "_top", Value = "_top"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountButtons = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Buttons")).Count();
            if ((ViewBag.CountButtons > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Buttons' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Buttons", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialButtonsIndex")]
        public ActionResult _PartialButtonsIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Button = db.Styles.Find(id);
            }

            ViewBag.PickButtonType = new List<SelectListItem>
            {
                new SelectListItem {Text = "Button", Value = "button"},
                new SelectListItem {Text = "Submit", Value = "submit"},
                new SelectListItem {Text = "Reset", Value = "reset"}
            };
            ViewBag.ButtonTarget = new List<SelectListItem>
            {
                new SelectListItem {Text = "_blank", Value = "_blank"},
                new SelectListItem {Text = "_self", Value = "_self"},
                new SelectListItem {Text = "_parent", Value = "_parent" },
                new SelectListItem {Text = "_top", Value = "_top"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountButtons = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Buttons")).Count();
            if ((ViewBag.CountButtons > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Buttons' to save!";
            return PartialView("_PartialButtonsIndex");
        }
        [HttpPost, ActionName("PartialButtons")]
        public ActionResult _PartialButtons(string PickButtonType, string ButtonText, string ButtonName, string ButtonTarget)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["PickButtonType"] = PickButtonType;
            TempData["ButtonText"] = ButtonText;
            TempData["ButtonName"] = ButtonName;
            TempData["ButtonTarget"] = ButtonTarget;
            return PartialView("_PartialButtons", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buttons(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialButtons");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Buttons";
                style.String1 = TempData["PickButtonType"] as string;
                style.String2 = TempData["ButtonText"] as string;
                style.String3 = TempData["ButtonName"] as string;
                style.String4 = TempData["ButtonTarget"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Buttons";
                styleOld.String1 = TempData["PickButtonType"] as string;
                styleOld.String2 = TempData["ButtonText"] as string;
                styleOld.String3 = TempData["ButtonName"] as string;
                styleOld.String4 = TempData["ButtonTarget"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialButtonsIndex");
        }

        public ActionResult Links(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Link = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.linkTarget = new List<SelectListItem>
            {
                new SelectListItem {Text = "_blank", Value = "_blank"},
                new SelectListItem {Text = "_self", Value = "_self"},
                new SelectListItem {Text = "_parent", Value = "_parent"},
                new SelectListItem {Text = "_top", Value = "_top"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountLinks = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Links")).Count();
            if ((ViewBag.CountLinks > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Links' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Links", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialLinksIndex")]
        public ActionResult _PartialLinksIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Link = db.Styles.Find(id);
            }

            ViewBag.linkTarget = new List<SelectListItem>
            {
                new SelectListItem {Text = "_blank", Value = "_blank"},
                new SelectListItem {Text = "_self", Value = "_self"},
                new SelectListItem {Text = "_parent", Value = "_parent"},
                new SelectListItem {Text = "_top", Value = "_top"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountLinks = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Links")).Count();
            if ((ViewBag.CountLinks > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Links' to save!";
            return PartialView("_PartialLinksIndex");
        }
        [HttpPost, ActionName("PartialLinks")]
        public ActionResult _PartialLinks(string LinkText, string LinkHref, string linkTarget)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["LinkText"] = LinkText;
            TempData["LinkHref"] = LinkHref;
            TempData["linkTarget"] = linkTarget;
            return PartialView("_PartialLinks", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Links(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialLinks");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Links";
                style.String1 = TempData["LinkText"] as string;
                style.String2 = TempData["LinkHref"] as string;
                style.String3 = TempData["linkTarget"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                TempData["style"] = "Style was saved!";
                db.Styles.Add(style);
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Links";
                styleOld.String1 = TempData["LinkText"] as string;
                styleOld.String2 = TempData["LinkHref"] as string;
                styleOld.String3 = TempData["linkTarget"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialLinksIndex");
        }

        public ActionResult Images(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Image = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountImages = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Images")).Count();
            if ((ViewBag.CountImages > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Images' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Images", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialImagesIndex")]
        public ActionResult _PartialImagesIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Image = db.Styles.Find(id);
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountImages = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Images")).Count();
            if ((ViewBag.CountImages > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Images' to save!";
            return PartialView("_PartialImagesIndex");
        }
        [HttpPost, ActionName("PartialImages")]
        public ActionResult _PartialImages(string ImageSource, string ImageAlt, string ImageWidth, string ImageHeight)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["ImageSource"] = ImageSource;
            TempData["ImageAlt"] = ImageAlt;
            TempData["ImageWidth"] = ImageWidth;
            TempData["ImageHeight"] = ImageHeight;
            return PartialView("_PartialImages", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Images(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialImages");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Images";
                style.String1 = TempData["ImageSource"] as string;
                style.String2 = TempData["ImageAlt"] as string;
                style.String3 = TempData["ImageWidth"] as string;
                style.String4 = TempData["ImageHeight"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Images";
                styleOld.String1 = TempData["ImageSource"] as string;
                styleOld.String2 = TempData["ImageAlt"] as string;
                styleOld.String3 = TempData["ImageWidth"] as string;
                styleOld.String4 = TempData["ImageHeight"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialImagesIndex");
        }

        public ActionResult Headings(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Heading = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.HeadingV = new List<SelectListItem>
            {
                new SelectListItem {Text = "h1", Value = "h1"},
                new SelectListItem {Text = "h2", Value = "h2"},
                new SelectListItem {Text = "h3", Value = "h3"},
                new SelectListItem {Text = "h4", Value = "h4"},
                new SelectListItem {Text = "h5", Value = "h5"},
                new SelectListItem {Text = "h6", Value = "h6"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountHeadings = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Headings")).Count();
            if ((ViewBag.CountHeadings > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Headings' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Headings", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialHeadingsIndex")]
        public ActionResult _PartialHeadingsIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Heading = db.Styles.Find(id);
            }

            ViewBag.HeadingV = new List<SelectListItem>
            {
                new SelectListItem {Text = "h1", Value = "h1"},
                new SelectListItem {Text = "h2", Value = "h2"},
                new SelectListItem {Text = "h3", Value = "h3"},
                new SelectListItem {Text = "h4", Value = "h4"},
                new SelectListItem {Text = "h5", Value = "h5"},
                new SelectListItem {Text = "h6", Value = "h6"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountHeadings = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Headings")).Count();
            if ((ViewBag.CountHeadings > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Headings' to save!";
            return PartialView("_PartialHeadingsIndex");
        }
        [HttpPost, ActionName("PartialHeadings")]
        public ActionResult _PartialHeadings(string HeadingText, string Heading)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["HeadingText"] = HeadingText;
            TempData["Heading"] = Heading;
            return PartialView("_PartialHeadings", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Headings(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialHeadings");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Headings";
                style.String1 = TempData["HeadingText"] as string;
                style.String2 = TempData["Heading"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Headings";
                styleOld.String1 = TempData["HeadingText"] as string;
                styleOld.String2 = TempData["Heading"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialHeadingsIndex");
        }

        public ActionResult Paragraphs(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.Paragraph = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.ParagraphStyle = new List<SelectListItem>
            {
                new SelectListItem {Text = "p", Value = "p"},
                new SelectListItem {Text = "pre", Value = "pre"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountParagraphs = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Paragraphs")).Count();
            if ((ViewBag.CountParagraphs > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Paragraphs' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Paragraphs", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialParagraphsIndex")]
        public ActionResult _PartialParagraphsIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.Paragraph = db.Styles.Find(id);
            }

            ViewBag.ParagraphStyle = new List<SelectListItem>
            {
                new SelectListItem {Text = "p", Value = "p"},
                new SelectListItem {Text = "pre", Value = "pre"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountParagraphs = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Paragraphs")).Count();
            if ((ViewBag.CountParagraphs > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Paragraphs' to save!";
            return PartialView("_PartialParagraphsIndex");
        }
        [HttpPost, ActionName("PartialParagraphs")]
        public ActionResult _PartialParagraphs(string ParagraphText, string ParagraphStyle)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["ParagraphText"] = ParagraphText;
            TempData["ParagraphStyle"] = ParagraphStyle;
            return PartialView("_PartialParagraphs", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paragraphs(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialParagraphs");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Paragraphs";
                style.String1 = TempData["ParagraphText"] as string;
                style.String2 = TempData["ParagraphStyle"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Paragraphs";
                styleOld.String1 = TempData["ParagraphText"] as string;
                styleOld.String2 = TempData["ParagraphStyle"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialParagraphsIndex");
        }

        public ActionResult UnorderedList(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.unorderedList = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.UlstyleType = new List<SelectListItem>
            {
                new SelectListItem {Text = "disk", Value = "disk"},
                new SelectListItem {Text = "circle", Value = "circle"},
                new SelectListItem {Text = "square", Value = "square"},
                new SelectListItem {Text = "none", Value = "none"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountUnorderedList = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("UnorderedList")).Count();
            if ((ViewBag.CountUnorderedList > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'UnorderedList' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("UnorderedList", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialUnorderedListIndex")]
        public ActionResult _PartialUnorderedListIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.unorderedList = db.Styles.Find(id);
            }

            ViewBag.UlstyleType = new List<SelectListItem>
            {
                new SelectListItem {Text = "disk", Value = "disk"},
                new SelectListItem {Text = "circle", Value = "circle"},
                new SelectListItem {Text = "square", Value = "square"},
                new SelectListItem {Text = "none", Value = "none"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountUnorderedList = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("UnorderedList")).Count();
            if ((ViewBag.CountUnorderedList > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'UnorderedList' to save!";
            return PartialView("_PartialUnorderedListIndex");
        }
        [HttpPost, ActionName("PartialUnorderedList")]
        public ActionResult _PartialUnorderedList(string ListText, string UlstyleType)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["ListText"] = ListText;
            TempData["UlstyleType"] = UlstyleType;
            return PartialView("_PartialUnorderedList", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnorderedList(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialUnorderedList");
            }
            if (Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "UnorderedList";
                style.String1 = TempData["ListText"] as string;
                style.String2 = TempData["UlstyleType"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "UnorderedList";
                styleOld.String1 = TempData["ListText"] as string;
                styleOld.String2 = TempData["UlstyleType"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialUnorderedListIndex");
        }

        public ActionResult OrderedList(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.orderedList = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.OlstyleType = new List<SelectListItem>
            {
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "a", Value = "a"},
                new SelectListItem {Text = "I", Value = "I"},
                new SelectListItem {Text = "i", Value = "i"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountOrderedList = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("OrderedList")).Count();
            if ((ViewBag.CountOrderedList > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'OrderedList' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("OrderedList", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialOrderedListIndex")]
        public ActionResult _PartialOrderedListIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.orderedList = db.Styles.Find(id);
            }

            ViewBag.OlstyleType = new List<SelectListItem>
            {
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "a", Value = "a"},
                new SelectListItem {Text = "I", Value = "I"},
                new SelectListItem {Text = "i", Value = "i"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountOrderedList = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("OrderedList")).Count();
            if ((ViewBag.CountOrderedList > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'OrderedList' to save!";
            return PartialView("_PartialOrderedListIndex");
        }
        [HttpPost, ActionName("PartialOrderedList")]
        public ActionResult _PartialOrderedList(string ListText, string OlstyleType)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["ListText"] = ListText;
            TempData["OlstyleType"] = OlstyleType;
            return PartialView("_PartialOrderedList", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderedList(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialOrderedList");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "OrderedList";
                style.String1 = TempData["ListText"] as string;
                style.String2 = TempData["OlstyleType"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "OrderedList";
                styleOld.String1 = TempData["ListText"] as string;
                styleOld.String2 = TempData["OlstyleType"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialOrderedListIndex");
        }

        public ActionResult Header(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.header = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.PickHeaderColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountHeader = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Header")).Count();
            if ((ViewBag.CountHeader > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Header' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Header", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialHeaderIndex")]
        public ActionResult _PartialHeaderIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.header = db.Styles.Find(id);
            }

            ViewBag.PickHeaderColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountHeader = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Header")).Count();
            if ((ViewBag.CountHeader > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Header' to save!";
            return PartialView("_PartialHeaderIndex");
        }
        [HttpPost, ActionName("PartialHeader")]
        public ActionResult _PartialHeader(string HeaderText, string PickHeaderColor)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["HeaderText"] = HeaderText;
            TempData["PickHeaderColor"] = PickHeaderColor;
            return PartialView("_PartialHeader", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Header(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialHeader");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Header";
                style.String1 = TempData["HeaderText"] as string;
                style.String2 = TempData["PickHeaderColor"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Header";
                styleOld.String1 = TempData["HeaderText"] as string;
                styleOld.String2 = TempData["PickHeaderColor"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialHeaderIndex");
        }

        public ActionResult Navigation(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.navigation = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.PickNavColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            ViewBag.PickNavHoverColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountNavigation = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Navigation")).Count();
            if ((ViewBag.CountNavigation > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Navigation' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Navigation", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialNavigationIndex")]
        public ActionResult _PartialNavigationIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.navigation = db.Styles.Find(id);
            }

            ViewBag.PickNavColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            ViewBag.PickNavHoverColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountNavigation = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Navigation")).Count();
            if ((ViewBag.CountNavigation > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Navigation' to save!";
            return PartialView("_PartialNavigationIndex");
        }
        [HttpPost, ActionName("PartialNavigation")]
        public ActionResult _PartialNavigation(string NavText, string PickNavColor, string PickNavHoverColor)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["NavText"] = NavText;
            TempData["PickNavColor"] = PickNavColor;
            TempData["PickNavHoverColor"] = PickNavHoverColor;
            return PartialView("_PartialNavigation", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Navigation(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialNavigation");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Navigation";
                style.String1 = TempData["NavText"] as string;
                style.String2 = TempData["PickNavColor"] as string;
                style.String3 = TempData["PickNavHoverColor"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Navigation";
                styleOld.String1 = TempData["NavText"] as string;
                styleOld.String2 = TempData["PickNavColor"] as string;
                styleOld.String3 = TempData["PickNavHoverColor"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialNavigationIndex");
        }

        public ActionResult Main(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.main = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountMain = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Main")).Count();
            if ((ViewBag.CountMain > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Main' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Main", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialMainIndex")]
        public ActionResult _PartialMainIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.main = db.Styles.Find(id);
            }
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountMain = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Main")).Count();
            if ((ViewBag.CountMain > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Main' to save!";

            return PartialView("_PartialMainIndex");
        }
        [HttpPost, ActionName("PartialMain")]
        public ActionResult _PartialMain(string SectionText, string ArticleText, string AsideText)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["SectionText"] = SectionText;
            TempData["ArticleText"] = ArticleText;
            TempData["AsideText"] = AsideText;
            return PartialView("_PartialMain", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Main(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialMain");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Main";
                style.String1 = TempData["SectionText"] as string;
                style.String2 = TempData["ArticleText"] as string;
                style.String3 = TempData["AsideText"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Main";
                styleOld.String1 = TempData["SectionText"] as string;
                styleOld.String2 = TempData["ArticleText"] as string;
                styleOld.String3 = TempData["AsideText"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialMainIndex");
        }

        public ActionResult Footer(int? id)
        {
            if (id != null)
            {
                Session["backUserId"] = id;
                ViewBag.footer = db.Styles.Find(id);
            }
            else
            {
                Session.Remove("backUserId");
            }

            ViewBag.FooterColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountFooter = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Footer")).Count();
            if ((ViewBag.CountFooter > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Footer' to save!";

            if (User.IsInRole("Admin"))
                return RedirectToAction("Footer", "Home", new { area = "Admin" });
            return View();
        }
        [HttpGet, ActionName("PartialFooterIndex")]
        public ActionResult _PartialFooterIndex()
        {
            int? id = Session["backUserId"] as int?;
            if (id != null)
            {
                ViewBag.footer = db.Styles.Find(id);
            }

            ViewBag.FooterColor = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"}
            };
            int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CountFooter = db.Styles.Where(u => u.UserId == userId).Where(x => x.Category.Contains("Footer")).Count();
            if ((ViewBag.CountFooter > 4) && (id == null))
                TempData["count"] = "You can't save more than 5 styles! Please delete from category 'Footer' to save!";
            return PartialView("_PartialFooterIndex");
        }
        [HttpPost, ActionName("PartialFooter")]
        public ActionResult _PartialFooter(string FooterText, string FooterColor)
        {
            int? id = Session["backUserId"] as int?;
            Style style = null;
            if (id != null)
            {
                style = db.Styles.Find(id);
            }
            TempData["FooterText"] = FooterText;
            TempData["FooterColor"] = FooterColor;
            return PartialView("_PartialFooter", style);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Footer(Style style)
        {
            if (string.IsNullOrEmpty(style.Title))
            {
                return PartialView("_PartialFooter");
            }
            if(Session["backUserId"] == null)
            {
                int userId = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
                style.Category = "Footer";
                style.String1 = TempData["FooterText"] as string;
                style.String2 = TempData["FooterColor"] as string;
                style.Time = DateTime.Now;
                style.UserId = userId;
                db.Styles.Add(style);
                TempData["style"] = "Style was saved!";
            }
            else
            {
                int? id = Session["backUserId"] as int?;
                var styleOld = db.Styles.Find(id);
                styleOld.Title = style.Title;
                styleOld.Category = "Footer";
                styleOld.String1 = TempData["FooterText"] as string;
                styleOld.String2 = TempData["FooterColor"] as string;
                styleOld.Time = DateTime.Now;
                db.Entry(styleOld).State = EntityState.Modified;
                TempData["style"] = "Style was Changed!";
            }
            db.SaveChanges();
            return RedirectToAction("PartialFooterIndex");
        }
    }
}