using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RunGymFront.Models
{
    public class Comidas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Range(1, 100)] 
        public int IdDieta { get; set; }

        [Required(ErrorMessage = "El tipo de comida es obligatorio.")]
        [StringLength(50)]
        public string TipoComida { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime HoraComida { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [ForeignKey("IdDieta")]
        public Dieta Dieta { get; set; }
    }
}