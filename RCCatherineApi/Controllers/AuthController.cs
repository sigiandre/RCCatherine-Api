using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Models;
using RCCatherineApi.Services;

namespace RCCatherineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("trabajadores")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistrarTrabajador([FromBody] RegistrarTrabajadorDto dto)
        {
            bool existe = await _context.Trabajadores
                .AnyAsync(t => t.Usuario == dto.Usuario || t.Dni == dto.Dni);

            if (existe)
                return Conflict(new { mensaje = "Ya existe un trabajador con ese usuario o DNI." });

            var trabajador = new Trabajador
            {
                NombreCompleto = dto.NombreCompleto,
                Dni = dto.Dni,
                Usuario = dto.Usuario,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = dto.Rol,
                Activo = true
            };

            _context.Trabajadores.Add(trabajador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(RegistrarTrabajador), new { id = trabajador.Id }, new
            {
                trabajador.Id,
                trabajador.NombreCompleto,
                trabajador.Usuario,
                trabajador.Rol
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(t => t.Usuario == request.Usuario && t.Activo);

            if (trabajador is null || !BCrypt.Net.BCrypt.Verify(request.Password, trabajador.PasswordHash))
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

            var (token, expiracion) = _jwtService.GenerarToken(trabajador);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Expiracion = expiracion,
                TrabajadorId = trabajador.Id,
                NombreCompleto = trabajador.NombreCompleto,
                Rol = trabajador.Rol
            });
        }
    }
}