﻿
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using CSharp_Mascotas2.DTO;
using CSharp_Mascotas2.Services.Interfaces;
using CSharp_Mascotas2.Entidades;
using CSharp_Mascotas2.Services;

namespace CSharp_Mascotas2.Controllers;




    //[EnableCors("extrados")]
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly IUserService userService;
        private IConfiguration config;

   
        public UsuarioController(IUserService userService, IConfiguration config)
        {
            this.userService = userService;
            this.config = config;
        }

        [HttpGet]
       // [Authorize(Roles = "1")]
        public IEnumerable<Usuario> Get()
        {
            return userService.Get();
        }



        [HttpPost]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Post([FromBody] usuarioDto createUserDto)
        {
            var newUser = await userService.CreateUserAsync(createUserDto);

            return CreatedAtAction(nameof(Get), new { id = createUserDto.Email }, newUser);
        }


        private string GenerateToken(Usuario user)
        {
            var claims = new[]{
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Id_rol.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(2000),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(loginDto login)
        {
            var user = await userService.Login(login);

            if (user == null)
            {
                return BadRequest(new { message = "Credenciales inválidas" });
            }

            // El login es exitoso, puedes realizar alguna acción adicional si es necesario
            string jwtToken = GenerateToken(user);
            return Ok(new { token = jwtToken });
        }




    }
