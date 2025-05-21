using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RunGymFront.Controllers
{
    public class DietasController : Controller
    {
        public ActionResult Keto()
        {
            var tokenCookie = Request.Cookies["BearerToken"];
            var tokenExpirationCookie = Request.Cookies["TokenExpirationTime"];
            var tokenSession = Session["BearerToken"] as string;

            Console.WriteLine("BearerToken Cookie: " + tokenCookie?.Value);
            Console.WriteLine("TokenExpirationTime Cookie: " + tokenExpirationCookie?.Value);

            string token = tokenCookie?.Value ?? tokenSession;

            // Redirigir si no hay token
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token no encontrado. Por favor inicia sesión.";
                return RedirectToAction("Login", "Login");
            }

            DateTime? expirationTime = null;
            if (tokenExpirationCookie != null)
            {
                expirationTime = DateTime.TryParse(tokenExpirationCookie.Value, out var expiryDate) ? expiryDate : (DateTime?)null;
                Console.WriteLine("Fecha de expiración parseada: " + expirationTime);
            }

            // Validar expiración
            if (expirationTime.HasValue && DateTime.UtcNow > expirationTime.Value.ToUniversalTime())
            {
                HttpContext.Response.Cookies.Remove("BearerToken");
                HttpContext.Response.Cookies.Remove("TokenExpirationTime");

                TempData["Error"] = "La sesión ha expirado. Por favor inicia sesión nuevamente.";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult Mediterranea()
        {
            var tokenCookie = Request.Cookies["BearerToken"];
            var tokenExpirationCookie = Request.Cookies["TokenExpirationTime"];
            var tokenSession = Session["BearerToken"] as string;

            Console.WriteLine("BearerToken Cookie: " + tokenCookie?.Value);
            Console.WriteLine("TokenExpirationTime Cookie: " + tokenExpirationCookie?.Value);

            string token = tokenCookie?.Value ?? tokenSession;

            // Redirigir si no hay token
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token no encontrado. Por favor inicia sesión.";
                return RedirectToAction("Login", "Login");
            }

            DateTime? expirationTime = null;
            if (tokenExpirationCookie != null)
            {
                expirationTime = DateTime.TryParse(tokenExpirationCookie.Value, out var expiryDate) ? expiryDate : (DateTime?)null;
                Console.WriteLine("Fecha de expiración parseada: " + expirationTime);
            }

            // Validar expiración
            if (expirationTime.HasValue && DateTime.UtcNow > expirationTime.Value.ToUniversalTime())
            {
                HttpContext.Response.Cookies.Remove("BearerToken");
                HttpContext.Response.Cookies.Remove("TokenExpirationTime");

                TempData["Error"] = "La sesión ha expirado. Por favor inicia sesión nuevamente.";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult Vegetariana()
        {
            var tokenCookie = Request.Cookies["BearerToken"];
            var tokenExpirationCookie = Request.Cookies["TokenExpirationTime"];
            var tokenSession = Session["BearerToken"] as string;

            Console.WriteLine("BearerToken Cookie: " + tokenCookie?.Value);
            Console.WriteLine("TokenExpirationTime Cookie: " + tokenExpirationCookie?.Value);

            string token = tokenCookie?.Value ?? tokenSession;

            // Redirigir si no hay token
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token no encontrado. Por favor inicia sesión.";
                return RedirectToAction("Login", "Login");
            }

            DateTime? expirationTime = null;
            if (tokenExpirationCookie != null)
            {
                expirationTime = DateTime.TryParse(tokenExpirationCookie.Value, out var expiryDate) ? expiryDate : (DateTime?)null;
                Console.WriteLine("Fecha de expiración parseada: " + expirationTime);
            }

            // Validar expiración
            if (expirationTime.HasValue && DateTime.UtcNow > expirationTime.Value.ToUniversalTime())
            {
                HttpContext.Response.Cookies.Remove("BearerToken");
                HttpContext.Response.Cookies.Remove("TokenExpirationTime");

                TempData["Error"] = "La sesión ha expirado. Por favor inicia sesión nuevamente.";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
