using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(20)]
        public string Identificacion { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [ValidateNever]
        public ICollection<Agendamentos> Agendamentos { get; set; }
    }
}
