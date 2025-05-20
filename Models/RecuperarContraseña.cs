using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RunGymFront.Models;

namespace RunGymFront.Models
{
    public class RecuperarContraseña
    {
        public string Codigo { get; set; }
        public string NuevaContraseña { get; set; }
        public string ConfirmarContraseña { get; set; }
    }
}