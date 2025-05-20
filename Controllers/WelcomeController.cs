using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RunGymFront.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: /Login/Welcome
        public ActionResult Welcome()
        {
            // Página mostrada a usuarios no autenticados o después de cerrar sesión
            return View();
        }
    }
}
