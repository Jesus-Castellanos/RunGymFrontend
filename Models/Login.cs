using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;

namespace RunGymFront.Models
{
    public class Login
    {
        [DisplayName("Correo")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress]
        public string Correo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Contraseña { get; set; }
    }
}