using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HTMLGeneratorMVC.Context;
using HTMLGeneratorMVC.Models;
using Microsoft.AspNet.Identity;

namespace HTMLGeneratorMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string message)
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string newMessage = null;
                if (user.Genter == "Male")
                    newMessage = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    newMessage = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = newMessage });
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            
            return View();
        }

        public ActionResult Styles()
        {
            
            ViewBag.PickBodyColorDropDown = new List<SelectListItem>
            {
                new SelectListItem {Text = "graphite", Value = "background-color:#333;"},
                new SelectListItem {Text = "red", Value = "background-color:red;"},
                new SelectListItem {Text = "blue", Value = "background-color:blue;"},
                new SelectListItem {Text = "green", Value = "background-color:green;"},
                new SelectListItem {Text = "pink", Value = "background-color:pink;"},
                new SelectListItem {Text = "yellow", Value = "background-color:yellow;"},
                new SelectListItem {Text = "black", Value = "background-color:black;"}
            };
            ViewBag.PickBodyTextSizeDropDown = new List<SelectListItem>
            {
                new SelectListItem {Text = "100%", Value = "font-size:100%"},
                new SelectListItem {Text = "150%", Value = "font-size:150%"},
                new SelectListItem {Text = "200%", Value = "font-size:200%"},
                new SelectListItem {Text = "300%", Value = "font-size:300%"},
                new SelectListItem {Text = "400%", Value = "font-size:400%"}
            };
            ViewBag.HeadingTextColorDropDown = new List<SelectListItem>
            {
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };
            ViewBag.ParagraphTextColorDropDown = new List<SelectListItem>
            {
                new SelectListItem {Text = "pink", Value = "color:pink;"},
                new SelectListItem {Text = "red", Value = "color:red;"},
                new SelectListItem {Text = "blue", Value = "color:blue;"},
                new SelectListItem {Text = "green", Value = "color:green;"},
                new SelectListItem {Text = "yellow", Value = "color:yellow;"},
                new SelectListItem {Text = "black", Value = "color:black;"}
            };

            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            return View();
        }

        public ActionResult Colors()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Tables()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Formatting()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Buttons()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Links()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Images()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }
        public ActionResult Headings()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Paragraphs()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult UnorderedList()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult OrderedList()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Header()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Navigation()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Main()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }

        public ActionResult Footer()
        {
            if (Request.IsAuthenticated)
            {
                GeneratorDb db = new GeneratorDb();
                var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                string message = null;
                if (user.Genter == "Male")
                    message = string.Format("Welcome Mr. {0}!", user.FirstName);
                else
                    message = string.Format("Welcome Ms. {0}!", user.FirstName);
                return RedirectToAction("Index", "Home", new { area = "User", message = message });
            }
            return View();
        }
    }
}