using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class AgendamentoPersonalizado
    {

        public int Id { get; set; } 

        [Required]
        public int ClienteId { get; set; }

        
        [StringLength(100)]
        [Display(Name = "Nome do Serviço")]
        public string? ServicoNome { get; set; }

        [StringLength(300)]
        [Display(Name = "Descrição do Serviço")]
        public string ServicoDescricao { get; set; }

       
        
        [Precision(18, 2)]
        [Display(Name = "Valor")]
        public decimal? ServicoValor { get; set; }

        [Required]
        [Display(Name = "Data do Agendamento")]
        public DateTime Data { get; set; }
    }
}
