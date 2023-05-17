using CSharp_Mascotas2.DTO;
using CSharp_Mascotas2.Entidades;

namespace CSharp_Mascotas2.Services.Interfaces;

    public interface IUserService
    {

        IEnumerable<Usuario> Get();
        Task<Usuario> CreateUserAsync(usuarioDto createUserDto);
        Task<Usuario> Login(loginDto login);

        // Otros métodos que necesites
    }
