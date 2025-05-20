using Newtonsoft.Json;
using RunGymFront.Models; // Asegúrate que este namespace exista y contenga Login y Token
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RunGymFront.Utils;

namespace RunGymFront.Controllers
{
    public class LoginController : Controller
    {
        string apiUrl = ConfigurationManager.AppSettings["Api"]?.ToString(); // Usar ?. para seguridad

        public ActionResult Login(string returnUrl) // Añadir returnUrl como parámetro
        {
            // Pasar returnUrl a la vista para que se pueda incluir en el formulario
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login model, string returnUrl) // Recibir returnUrl del formulario
        {
            // Validar si la URL de la API está configurada
            if (string.IsNullOrEmpty(apiUrl))
            {
                ModelState.AddModelError("", "La configuración de la API no está disponible.");
                return View(model); // Devolver vista con error
            }

            // 1. Manejo de ModelState Inválido
            if (!ModelState.IsValid)
            {
                // Devuelve la vista con el modelo para mostrar los errores de validación
                return View(model);
            }

            Token token = null; // Inicializar como null

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    // Asegúrate que tu modelo Login tenga las propiedades correctas (ej: Correo, Contraseña)
                    string hash = Encriptador.Encriptar(model.Contraseña);
                    System.Diagnostics.Debug.WriteLine("🔒 HASH GENERADO DESDE FRONT: " + hash);
                    model.Contraseña = hash;

                    string json = JsonConvert.SerializeObject(new { Correo = model.Correo, Contraseña = model.Contraseña });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("api/Auth/Login", content); // Asumiendo que esta es la ruta correcta

                    if (Res.IsSuccessStatusCode)
                    {
                        var res = await Res.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject<Token>(res); // Asumiendo que Token tiene una propiedad 'token'

                        if (token != null && !string.IsNullOrEmpty(token.token))
                        {
                            // 3. Llamar a CookieUpdate (mejorada)
                            // Pasamos solo la información necesaria y segura (ej. email, quizás un ID de usuario si la API lo devuelve)
                            // Asumiendo que el token contiene información del usuario o necesitas hacer otra llamada para obtenerla.
                            // Por simplicidad, usaremos el Correo como identificador en la cookie.
                            CookieUpdate(model.Correo); // Pasar solo el correo (o ID de usuario si lo tienes)

                            Session["BearerToken"] = token.token; // Guardar el token en sesión
                            Session["UserEmail"] = model.Correo; // Opcional: guardar email en sesión si es útil

                            // 4. Redirección con returnUrl
                            if (Url.IsLocalUrl(returnUrl)) // Medida de seguridad contra Open Redirect Attacks
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home"); // Redirección por defecto
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "La respuesta de la API no contiene un token válido.");
                        }
                    }
                    else
                    {
                        // 2. Manejo de Fallo en la API
                        // Intentar leer el mensaje de error de la API si existe
                        string apiError = await Res.Content.ReadAsStringAsync();
                        string errorMessage = $"Error de autenticación: {Res.ReasonPhrase}";
                        if (!string.IsNullOrWhiteSpace(apiError))
                        {
                            // Podrías intentar deserializar un objeto de error específico de tu API
                            errorMessage += $" ({apiError})";
                        }
                        ModelState.AddModelError("", errorMessage); // Agregar error genérico o específico de la API
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar excepciones de red u otras
                ModelState.AddModelError("", $"Ha ocurrido un error inesperado: {ex.Message}");
                // Considera loggear el error completo (ex) para diagnóstico
            }


            // Si llegamos aquí, el login falló (API devolvió error, token inválido, excepción)
            return View(model); // Devolver la vista de login con los errores
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize] // Es buena idea proteger LogOff para que solo usuarios autenticados lo llamen
        public ActionResult LogOff()
        {
            Session.Clear(); // Usar Clear() es más seguro que RemoveAll() en algunos contextos
            Session.Abandon(); // Invalida la sesión
            FormsAuthentication.SignOut(); // Elimina la cookie de autenticación

            // Limpiar cookies de sesión explícitamente si es necesario
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var expiredCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true // Mantener HttpOnly si estaba configurado
                };
                Response.Cookies.Add(expiredCookie);
            }
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var expiredSessionCookie = new HttpCookie("ASP.NET_SessionId")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(expiredSessionCookie);
            }


            // Redirigir a una página pública, como Welcome o Login
            return RedirectToAction("Login", "Login"); // O "Welcome" si prefieres
        }

        // 3. CookieUpdate Mejorada
        private void CookieUpdate(string userIdentifier) // Recibe solo el identificador (email, username, id)
        {
            // Crear el ticket de autenticación.
            // Usar userIdentifier (ej. email) como Name.
            // NO incluir la contraseña.
            // En UserData puedes poner roles u otra info segura separada por comas, si la tienes.
            string userData = ""; // Ejemplo: "Admin,Editor" si tuvieras roles

            var ticket = new FormsAuthenticationTicket(
                2,                                      // version
                userIdentifier,                         // name (identificador del usuario)
                DateTime.Now,                           // issueDate
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), // expiration
                false,                                  // isPersistent (false = cookie de sesión)
                                                        // Si quieres "Recordarme", esto debería ser true y el timeout más largo.
                userData                                // userData (roles u otra info NO SENSIBLE)
            );

            // Encriptar el ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Crear la cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true, // Ayuda a prevenir ataques XSS
                Secure = Request.IsSecureConnection, // Enviar solo sobre HTTPS si está disponible
                Expires = ticket.IsPersistent ? ticket.Expiration : DateTime.MinValue // Establecer expiración si es persistente
            };

            // Agregar la cookie a la respuesta
            Response.AppendCookie(cookie);

            // ELIMINAR: Ya no guardamos la contraseña en sesión
            // Session["Correo"] = usuario.Contraseña; // ¡¡¡Eliminar esta línea!!!
        }
    }

    // --- Modelos (Asegúrate que existan en RunGymFront.Models) ---
    // namespace RunGymFront.Models
    // {
    //     public class Login
    //     {
    //         [Required(ErrorMessage = "El correo es obligatorio")]
    //         [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    //         public string Correo { get; set; }
    //
    //         [Required(ErrorMessage = "La contraseña es obligatoria")]
    //         [DataType(DataType.Password)]
    //         public string Contraseña { get; set; }
    //     }
    //
    //     public class Token
    //     {
    //         public string token { get; set; }
    //         // Podrías tener otras propiedades que devuelve tu API, como 'expiresIn', 'userId', 'roles', etc.
    //     }
    // }

}