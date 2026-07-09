using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class Agendamentos
    {
        public int Id { get; set; }

        [Required]
        public int? ClienteId { get; set; }
        [ValidateNever]
        public Cliente Cliente { get; set; }

        [Required]
        public int? ServicosId { get; set; }
        [ValidateNever]
        public Servicos Servicos { get; set; }

        
        [StringLength(500)]
        public string? DescricaoPersonalizada { get; set; }



        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public EstadoAgendamentos Estado { get; set; }
    }
}
