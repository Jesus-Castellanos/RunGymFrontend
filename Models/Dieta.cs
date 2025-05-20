using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RunGymFront.Models
{
    public class Dieta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }


        [Required(ErrorMessage = "El tipo de dieta es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de dieta no puede exceder los 50 caracteres.")]
        public string TipoDieta { get; set; }


        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }


        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }


        [Range(0, 5000, ErrorMessage = "Las calorías diarias deben estar entre 0 y 5000.")]
        public int CaloriasDiarias { get; set; }

        [Required(ErrorMessage = "Los macronutrientes son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los macronutrientes no pueden exceder los 100 caracteres.")]
        public string Macronutrientes { get; set; }


        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")] 
        public string Descripcion { get; set; }
    }
}