using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class Veiculo
    {
        public int VeiculoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        [ValidateNever]
        public Cliente Cliente { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [StringLength(50)]
        [Display(Name = "Cor")]
        public string Cor { get; set; }

        [StringLength(50)]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        public int? Ano { get; set; }
    }
}
