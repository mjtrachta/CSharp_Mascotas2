using System.ComponentModel.DataAnnotations;

namespace CSharp_Mascotas2.Entidades
{
    public class Usuario
    {

        [Key]
        public int Id_usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id_rol { get; set; }
        public long? Dni { get; set; }
        public string Numero_telefono { get; set; }
    }
}