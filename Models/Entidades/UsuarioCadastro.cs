using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_III.Models.Entidades
{
    public class UsuarioCadastro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required, EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarPassword { get; set; }
    }
}
