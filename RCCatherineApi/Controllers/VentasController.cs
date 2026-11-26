using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Services;
using System.Security.Claims;

namespace RCCatherineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly VentaFacturacionService _ventaService;
        private readonly AppDbContext _context;

        public VentasController(VentaFacturacionService ventaService, AppDbContext context)
        {
            _ventaService = ventaService;
            _context = context;
        }

        // --- COTIZACIONES ---
        [HttpGet("cotizaciones")]
        public async Task<IActionResult> ListarCotizaciones()
        {
            var lista = await _context.Cotizaciones
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Include(c => c.DireccionEntrega)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.FechaEmision)
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost("cotizaciones")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> CrearCotizacion([FromBody] CrearCotizacionDto dto)
        {
            int vendedorId = ObtenerTrabajadorId();
            var cotizacion = await _ventaService.CrearCotizacionAsync(dto, vendedorId);
            return CreatedAtAction(nameof(ListarCotizaciones), new { id = cotizacion.Id }, cotizacion);
        }

        [HttpPatch("cotizaciones/{id:int}/estado")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> CambiarEstadoCotizacion(int id, [FromBody] CambiarEstadoCotizacionDto dto)
        {
            var cotizacion = await _context.Cotizaciones
                .Include(c => c.OrdenVenta)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cotizacion is null)
                return NotFound(new { mensaje = "Cotización no encontrada." });

            if (cotizacion.OrdenVenta is not null)
                return BadRequest(new { mensaje = "No se puede modificar una cotización ya vinculada a una Orden de Venta." });

            cotizacion.Estado = dto.Estado;
            await _context.SaveChangesAsync();

            return Ok(cotizacion);
        }

        // --- ÓRDENES DE VENTA ---
        [HttpGet("ordenes")]
        public async Task<IActionResult> ListarOrdenes()
        {
            var ordenes = await _context.OrdenesVenta
                .AsNoTracking()
                .Include(o => o.Cliente)
                .Include(o => o.DireccionEntrega)
                .Include(o => o.Pagos)
                .Include(o => o.ComprobanteElectronico)
                .Include(o => o.GuiaRemision)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return Ok(ordenes);
        }

        [HttpPost("ordenes")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> CrearOrdenVenta([FromBody] CrearOrdenVentaDto dto)
        {
            int vendedorId = ObtenerTrabajadorId();
            var orden = await _ventaService.CrearOrdenVentaAsync(dto, vendedorId);
            return CreatedAtAction(nameof(ListarOrdenes), new { id = orden.Id }, orden);
        }

        // --- COBROS Y CIERRE DE CAJA ---
        [HttpPost("pagos")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> RegistrarPagos([FromBody] RegistrarPagosOrdenDto dto)
        {
            int trabajadorId = ObtenerTrabajadorId();
            var orden = await _ventaService.RegistrarPagosAsync(dto, trabajadorId);
            return Ok(orden);
        }

        [HttpGet("cierre-caja")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> ObtenerCierreCaja([FromQuery] DateTime? fecha)
        {
            var dia = (fecha ?? DateTime.UtcNow).Date;
            var siguienteDia = dia.AddDays(1);

            var pagosDelDia = await _context.PagosVenta
                .AsNoTracking()
                .Where(p => p.FechaPago >= dia && p.FechaPago < siguienteDia)
                .ToListAsync();

            var resumenPorMetodo = pagosDelDia
                .GroupBy(p => p.MetodoPago)
                .Select(g => new
                {
                    MetodoPago = g.Key,
                    CantidadPagos = g.Count(),
                    TotalMonto = g.Sum(x => x.Monto)
                })
                .ToList();

            return Ok(new
            {
                Fecha = dia.ToString("yyyy-MM-dd"),
                TotalGeneral = pagosDelDia.Sum(p => p.Monto),
                DetallePorMetodo = resumenPorMetodo
            });
        }

        // --- FACTURACIÓN ELECTRÓNICA ---
        [HttpPost("comprobantes")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> EmitirComprobante([FromBody] EmitirComprobanteDto dto)
        {
            var comprobante = await _ventaService.EmitirComprobanteAsync(dto);
            return Ok(comprobante);
        }

        // --- DESPACHO / GUÍA DE REMISIÓN ---
        [HttpGet("pendientes-despacho")]
        [Authorize(Roles = "Almacenero,Administrador")]
        public async Task<IActionResult> ListarPendientesDespacho()
        {
            var pendientes = await _context.OrdenesVenta
                .AsNoTracking()
                .Include(o => o.Cliente)
                .Include(o => o.DireccionEntrega)
                .Include(o => o.ComprobanteElectronico)
                .Where(o => o.Estado == "FACTURADA" && o.GuiaRemision == null)
                .OrderBy(o => o.Fecha)
                .ToListAsync();

            return Ok(pendientes);
        }

        [HttpPost("guias-remision")]
        [Authorize(Roles = "Almacenero,Administrador")]
        public async Task<IActionResult> EmitirGuiaRemision([FromBody] EmitirGuiaRemisionDto dto)
        {
            int almacenistaId = ObtenerTrabajadorId();
            var guia = await _ventaService.EmitirGuiaRemisionAsync(dto, almacenistaId);
            return Ok(guia);
        }

        [HttpPatch("ordenes/{ordenId:int}/tracking-agencia")]
        [Authorize(Roles = "Almacenero,Administrador")]
        public async Task<IActionResult> ActualizarTrackingAgencia(int ordenId, [FromBody] ActualizarTrackingGuiaDto dto)
        {
            var orden = await _context.OrdenesVenta.FindAsync(ordenId);
            if (orden is null)
                return NotFound(new { mensaje = "Orden de venta no encontrada." });

            orden.NumeroGuiaAgencia = dto.NumeroGuiaAgencia.Trim();
            await _context.SaveChangesAsync();

            return Ok(orden);
        }

        private int ObtenerTrabajadorId()
        {
            var claim = User.FindFirstValue("trabajadorId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(claim!);
        }
    }
}