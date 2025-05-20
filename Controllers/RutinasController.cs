using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RunGymFront.Models;

namespace RunGymFront.Controllers
{
    public class RutinasController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["Api"].ToString();
        // GET: Rutinas
        public ActionResult Sentadillas()
        {
            return View();
        }

        // GET: Rutinas/Details/5
        public async Task<ActionResult> Flexiones(int id)
        {
            EjercicioDto? ejercicio = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);

                    HttpResponseMessage response = await client.GetAsync($"api/Ejercicios/GetDetalles/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error en la peticion");
                        TempData["Error"] = "No se pudo obtener el ejercicio.";
                        return View();
                    }

                    var res = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(res);

                    ejercicio = JsonConvert.DeserializeObject<EjercicioDto>(res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                TempData["Error"] = $"Error en la conexión con la API: {ex.Message}";
            }

            return View(ejercicio);
        }


        // GET: Rutinas/Create
        public ActionResult Hombros()
        {
            return View();
        }

        // GET: Rutinas/Create
        public ActionResult Lunges()
        {
            return View();
        }

        // GET: Rutinas/Create
        public ActionResult Biceps()
        {
            return View();
        }

        // GET: Rutinas/Create
        public ActionResult Espalda()
        {
            return View();
        }

        // GET: Rutinas/Create
        public ActionResult Abdominales()
        {
            return View();
        }

        // GET: Rutinas/Create
        public ActionResult Triceps()
        {
            return View();
        }
    }
}
