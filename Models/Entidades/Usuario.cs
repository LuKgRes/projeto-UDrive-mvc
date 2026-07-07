using Proyecto_Programacion_III.Models.Entidades.Opciones;


namespace Proyecto_Programacion_III.Models.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }

        public EstadoUsuario Estado { get; set; }

        public ICollection<Agendamentos> Agendamentos { get; set; }

        public Usuario()
        {
            Agendamentos = new List<Agendamentos>();
        }
    }
}
