using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class AgendamentoPersonalizado
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome do Serviço")]
        public string ServicoNome { get; set; }

        [StringLength(300)]
        [Display(Name = "Descrição do Serviço")]
        public string ServicoDescricao { get; set; }

        [Required]
        [Range(0.01, 999999)]
        [Display(Name = "Valor")]
        public decimal ServicoValor { get; set; }

        [Required]
        [Display(Name = "Data do Agendamento")]
        public DateTime Data { get; set; }
    }
}
