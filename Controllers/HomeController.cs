using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var alumnos = context.DbAlumnos.ToList();
            
            return View(alumnos);
        }
    }
}
