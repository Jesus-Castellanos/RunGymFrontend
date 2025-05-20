using Newtonsoft.Json;
using RunGymFront.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RunGymFront.Controllers
{
    public class HomeController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["Api"].ToString();

        public ActionResult Index()
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


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ErroresReportados model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver los errores
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new { success = false, message = "Por favor corrige los errores en el formulario.", errors });
                }

                // Asignar fecha actual si no viene en el modelo
                if (model.FechaReporte == default)
                {
                    model.FechaReporte = DateTime.Now;
                }

                // Si el usuario está autenticado, podemos asignar su ID
                if (User.Identity.IsAuthenticated)
                {
                    // Aquí asigna el ID del usuario si es necesario
                    // model.IdUsuario = ...;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/ErroresReportados/PostErroresReportados", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "¡Reporte enviado con éxito! Hemos recibido tu reporte y lo revisaremos pronto." });
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return Json(new
                        {
                            success = false,
                            message = "Error al enviar el reporte. Por favor, inténtalo de nuevo más tarde.",
                            apiError = errorContent
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Loggear el error (implementa tu propio sistema de logging)
                // _logger.LogError(ex, "Error al procesar el formulario de contacto");

                return Json(new
                {
                    success = false,
                    message = $"Ocurrió un error inesperado: {ex.Message}"
                });
            }
        }

        public async Task<ActionResult> Rutinas()
        {
            List<Ejercicios> ejercicios = new List<Ejercicios>();

            // Verificar si el token está en las cookies
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

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);

                    // Hacer la petición GET
                    HttpResponseMessage response = await client.GetAsync("api/Ejercicios/GetEjercicios");

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error en la peticion");
                    }

                    var res = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(res);
                    ejercicios = JsonConvert.DeserializeObject<List<Ejercicios>>(res) ?? new List<Ejercicios>();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                TempData["Error"] = $"Error en la conexión con la API: {ex.Message}";
                return RedirectToAction("Iniciar", "Home");
            }

            return View(ejercicios);
        }

        public ActionResult Dieta()
        {
            // Verificar si el token está en las cookies
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