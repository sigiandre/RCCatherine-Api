using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Models;
using RCCatherineApi.Repositories;
using RCCatherineApi.Services;
using System.Security.Claims;

namespace RCCatherineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventarioController : ControllerBase
    {
        private readonly InventarioService _inventarioService;
        private readonly IProductoRepository _productoRepo;
        private readonly AppDbContext _context;

        public InventarioController(InventarioService inventarioService, IProductoRepository productoRepo, AppDbContext context)
        {
            _inventarioService = inventarioService;
            _productoRepo = productoRepo;
            _context = context;
        }

        [HttpGet("ingresos")]
        public async Task<IActionResult> ListarIngresos()
        {
            var ingresos = await _context.Ingresos
                .AsNoTracking()
                .Include(i => i.Proveedor)
                .Include(i => i.Trabajador)
                .Include(i => i.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();

            return Ok(ingresos);
        }

        [HttpPost("ingresos")]
        [Authorize(Roles = "Almacenero,Administrador")]
        public async Task<IActionResult> RegistrarIngreso([FromBody] CrearIngresoDto dto)
        {
            int trabajadorId = ObtenerTrabajadorId();
            var ingreso = await _inventarioService.RegistrarIngresoAsync(dto, trabajadorId);
            return CreatedAtAction(nameof(ListarIngresos), new { id = ingreso.Id }, ingreso);
        }

        [HttpPost("ajustes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistrarAjuste([FromBody] AjusteInventarioDto dto)
        {
            int trabajadorId = ObtenerTrabajadorId();
            var mov = await _inventarioService.RegistrarAjusteManualAsync(dto, trabajadorId);
            return Ok(mov);
        }

        [HttpGet("alertas-stock-minimo")]
        public async Task<IActionResult> ObtenerAlertasStockMinimo()
        {
            var alertas = await _productoRepo.GetAlertasStockMinimoAsync();
            return Ok(alertas);
        }

        [HttpGet("kardex/{productoId:int}")]
        public async Task<IActionResult> ObtenerKardex(int productoId)
        {
            var movimientos = await _productoRepo.GetKardexPorProductoAsync(productoId);
            return Ok(movimientos);
        }

        private int ObtenerTrabajadorId()
        {
            var claim = User.FindFirstValue("trabajadorId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(claim!);
        }

        [HttpGet("proveedores")]
        public async Task<IActionResult> ListarProveedores()
        {
            var proveedores = await _context.Proveedores
                .AsNoTracking()
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return Ok(proveedores);
        }

        [HttpPost("proveedores")]
        [Authorize(Roles = "Almacenero,Administrador")]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.Ruc) || string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                return BadRequest("RUC y Razón Social son obligatorios.");

            proveedor.Id = 0;
            proveedor.Ruc = proveedor.Ruc.Trim();
            proveedor.RazonSocial = proveedor.RazonSocial.Trim();
            proveedor.Direccion = string.IsNullOrWhiteSpace(proveedor.Direccion) ? null : proveedor.Direccion.Trim();
            proveedor.Telefono = string.IsNullOrWhiteSpace(proveedor.Telefono) ? null : proveedor.Telefono.Trim();
            proveedor.Contacto = string.IsNullOrWhiteSpace(proveedor.Contacto) ? null : proveedor.Contacto.Trim();
            proveedor.Activo = true;

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarProveedores), new { id = proveedor.Id }, proveedor);
        }
    }
}