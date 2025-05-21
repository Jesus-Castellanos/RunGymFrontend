using Newtonsoft.Json;
using RunGymFront.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RunGymFront.controllers
{
    public class MetasController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["Api"].ToString();

        public async Task<ActionResult> MisMetas()
        {
            List<Metas> metas = new List<Metas>();
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
                    HttpResponseMessage response = await client.GetAsync("api/Metas/GetMetas");

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error en la peticion");
                    }

                    var res = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(res);
                    metas = JsonConvert.DeserializeObject<List<Metas>>(res) ?? new List<Metas>();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                TempData["Error"] = $"Error en la conexión con la API: {ex.Message}";
                return RedirectToAction("Iniciar", "Home");
            }
            var userId = GetLoggedInUserId(token);
            metas = metas.Where(m => m.IdUsuario.ToString() == userId).ToList();

            return View(metas);
        }

        public ActionResult Metas()
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

        // POST: Registro/Metas
        [HttpPost]
        public async Task<ActionResult> MetasCreate(Metas model)
        {
            try
            {
                using (var client = new HttpClient())
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

                    string userId = GetLoggedInUserId(token);
                    model.IdUsuario = int.Parse(userId);
                    Console.Write(model);
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    string json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/Metas/PostMetas", content);

                    // Verificar el código de estado de la respuesta
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // Lógica para guardar el usuario...

                    return RedirectToAction("MisMetas", "Metas");
                }
            }
            catch (Exception ex)
            {
                // Si algo sale mal en el código, capturar el error
                TempData["Error"] = $"Hubo un error al procesar la solicitud: {ex.Message}";
                return RedirectToAction("Metas", "Metas");
            }
        }

        public string GetLoggedInUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return userIdClaim?.Value;
        }
    }
}