
using System.Security.Cryptography;
using System.Text;
using CSharp_Mascotas2;
using CSharp_Mascotas2.DTO;
using CSharp_Mascotas2.Entidades;
using CSharp_Mascotas2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharp_Mascotas2.Services;

    public class UserService : IUserService
    {
        private readonly AppDbContext context;

        public UserService(AppDbContext context)
        {
            this.context = context;
        }

        public UserService()
        {

        }


        public IEnumerable<Usuario> Get()
        {
            return context.Usuarios2.Where(u => u.Dni != null && u.Email != null && u.Numero_telefono != null).ToList();
        }

        public static string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        public async Task<Usuario> CreateUserAsync(usuarioDto createUserDto)

        {
            // Hashear la contraseña
            string hashedPassword = HashPassword(createUserDto.Password);

            var newUser = new Usuario();

            newUser.Id_usuario = createUserDto.Id_usuario;
            newUser.Password = hashedPassword;
            newUser.Id_rol = createUserDto.Id_rol;
            newUser.Dni = createUserDto.Dni;
            newUser.Email = createUserDto.Email;
            newUser.Numero_telefono = createUserDto.Numero_telefono;

            // Agregar el nuevo usuario al DbContext y guardar los cambios
            context.Usuarios2.Add(newUser);
            await context.SaveChangesAsync();

            return newUser;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedInput = sha.ComputeHash(asByteArray);
            var hashedInputString = Convert.ToBase64String(hashedInput);

            return hashedInputString == hashedPassword;
        }

        public async Task<Usuario> Login(loginDto userLogin)
        {
            // Buscar el usuario por nombre de usuario
            var user = await context.Usuarios2.SingleOrDefaultAsync(x => x.Email == userLogin.Email);

            // Verificar si se encontró un usuario y si la contraseña coincide
            if (user != null && VerifyPassword(userLogin.Password, user.Password))
            {
                return user; // El login es exitoso, devolver el usuario
            }

            return null; // El login ha fallado
        }



    }
