using Microsoft.IdentityModel.Tokens;
using RCCatherineApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RCCatherineApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime Expiracion) GenerarToken(Trabajador trabajador)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Falta Jwt:Key en configuración.");
            var issuer = _configuration["Jwt:Issuer"] ?? "RcCatherine.Api";
            var audience = _configuration["Jwt:Audience"] ?? "RcCatherine.Web";
            var expiracion = DateTime.UtcNow.AddHours(8);

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, trabajador.Id.ToString()),
            new("trabajadorId", trabajador.Id.ToString()),
            new(ClaimTypes.Name, trabajador.NombreCompleto),
            new("usuario", trabajador.Usuario),
            new(ClaimTypes.Role, trabajador.Rol)
        };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiracion,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return (token, expiracion);
        }
    }
}