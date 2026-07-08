using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class Servicos
    {
        [Key]
        public int ServicosId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(300)]
        public string Descricao { get; set; }

        [Required]
        public DateTime Tempo { get; set; }

        [Required]
        [Range(0.01, 999999)]
        [Precision(18, 2)]
        public decimal Valor { get; set; }

        [Required]
        public EstadoServicos Estado { get; set; }

        public ICollection<Agendamentos> Agendamentos { get; set; }

        public Servicos()
        {
            Agendamentos = new List<Agendamentos>();
        }
    }
}
