using Newtonsoft.Json;
using RunGymFront.Models;
using RunGymFront.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RunGymFront.Controllers
{
    public class RegistroController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["Api"].ToString();

        // GET: Registro
        public ActionResult Registro()
        {
            return View();
        }

        // POST: Registro/Registro
        [HttpPost]
        public async Task<ActionResult> Registro(Usuarios model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    model.Contraseña = Encriptador.Encriptar(model.Contraseña);

                    string json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/Usuarios/PostUsuarios", content);

                    // Verificar el código de estado de la respuesta
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // Lógica para guardar el usuario...

                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                // Si algo sale mal en el código, capturar el error
                TempData["Error"] = $"Hubo un error al procesar la solicitud: {ex.Message}";
                return RedirectToAction("Registro", "Registro");
            }
        }
    }
}