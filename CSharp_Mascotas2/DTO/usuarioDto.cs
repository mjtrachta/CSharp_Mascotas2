namespace CSharp_Mascotas2.DTO
{
    public class usuarioDto
    {

        public int Id_usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id_rol { get; set; }
        public long? Dni { get; set; }
        public string Numero_telefono { get; set; }
    }
}