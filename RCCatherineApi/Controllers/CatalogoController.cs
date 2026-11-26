using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Data;
using RCCatherineApi.DTOs;
using RCCatherineApi.Models;
using RCCatherineApi.Repositories;

namespace RCCatherineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepo;

        public CatalogoController(AppDbContext context, IProductoRepository productoRepo)
        {
            _context = context;
            _productoRepo = productoRepo;
        }

        // --- UNIDADES SUNAT ---
        [HttpGet("unidades-sunat")]
        public async Task<IActionResult> ListarUnidadesSunat()
        {
            var unidades = await _context.UnidadesMedidaSunat
                .AsNoTracking()
                .Where(u => u.Activo)
                .OrderBy(u => u.CodigoSunat)
                .ToListAsync();

            return Ok(unidades);
        }

        // --- PRODUCTOS Y EQUIVALENCIAS ---
        [HttpGet("productos")]
        public async Task<IActionResult> ListarProductos()
        {
            var productos = await _productoRepo.GetCatalogoCompletoAsync();
            return Ok(productos);
        }

        [HttpPost("productos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            producto.Id = 0;
            producto.StockReservado = 0;
            producto.Activo = true;

            await _productoRepo.AddAsync(producto);
            await _productoRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarProductos), new { id = producto.Id }, producto);
        }

        [HttpPut("productos/{id:int}/precios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ActualizarPreciosCatalogo(int id, [FromBody] ActualizarPreciosCatalogoDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto is null)
                return NotFound(new { mensaje = "Producto no encontrado." });

            producto.PrecioMenor = dto.PrecioMenor;
            producto.PrecioMayor = dto.PrecioMayor;

            await _context.SaveChangesAsync();
            return Ok(producto);
        }

        [HttpPost("equivalencias")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarEquivalencia([FromBody] CrearEquivalenciaDto dto)
        {
            if (dto.FactorUnidadesBase <= 0)
                return BadRequest(new { mensaje = "El factor de conversión debe ser mayor a cero." });

            var eq = new EquivalenciaProducto
            {
                ProductoId = dto.ProductoId,
                UnidadMedidaSunatId = dto.UnidadMedidaSunatId,
                FactorUnidadesBase = dto.FactorUnidadesBase,
                Activo = true
            };

            _context.EquivalenciasProducto.Add(eq);
            await _context.SaveChangesAsync();

            return Ok(eq);
        }

        // --- CLIENTES Y DIRECCIONES ---
        [HttpGet("clientes")]
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Include(c => c.Direcciones.Where(d => d.Activo))
                .Where(c => c.Activo)
                .OrderBy(c => c.NombreRazonSocial)
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpPost("clientes")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDto dto)
        {
            var cliente = new Cliente
            {
                TipoDocumento = dto.TipoDocumento.ToUpper(),
                NumeroDocumento = dto.NumeroDocumento.Trim(),
                NombreRazonSocial = dto.NombreRazonSocial.Trim(),
                DireccionFiscal = dto.DireccionFiscal,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Activo = true
            };

            if (!string.IsNullOrWhiteSpace(dto.DireccionFiscal))
            {
                cliente.Direcciones.Add(new DireccionCliente
                {
                    Etiqueta = "Dirección Fiscal",
                    Direccion = dto.DireccionFiscal.Trim(),
                    EsDireccionFiscal = true,
                    Activo = true
                });
            }

            foreach (var dir in dto.DireccionesAdicionales)
            {
                cliente.Direcciones.Add(new DireccionCliente
                {
                    Etiqueta = dir.Etiqueta,
                    Direccion = dir.Direccion,
                    Ubigeo = dir.Ubigeo,
                    Referencia = dir.Referencia,
                    EsDireccionFiscal = dir.EsDireccionFiscal,
                    Activo = true
                });
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarClientes), new { id = cliente.Id }, cliente);
        }

        [HttpPost("clientes/{clienteId:int}/direcciones")]
        [Authorize(Roles = "Vendedor,Administrador")]
        public async Task<IActionResult> AgregarDireccionCliente(int clienteId, [FromBody] CrearDireccionClienteDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente is null)
                return NotFound(new { mensaje = "Cliente no encontrado." });

            var direccion = new DireccionCliente
            {
                ClienteId = clienteId,
                Etiqueta = dto.Etiqueta,
                Direccion = dto.Direccion,
                Ubigeo = dto.Ubigeo,
                Referencia = dto.Referencia,
                EsDireccionFiscal = dto.EsDireccionFiscal,
                Activo = true
            };

            _context.DireccionesCliente.Add(direccion);
            await _context.SaveChangesAsync();

            return Ok(direccion);
        }

        // --- AGENCIAS DE TRANSPORTE ---
        [HttpGet("agencias-transporte")]
        public async Task<IActionResult> ListarAgencias()
        {
            var agencias = await _context.AgenciasTransporte
                .AsNoTracking()
                .Where(a => a.Activo)
                .OrderBy(a => a.RazonSocial)
                .ToListAsync();

            return Ok(agencias);
        }

        [HttpPost("agencias-transporte")]
        [Authorize(Roles = "Administrador,Almacenero")]
        public async Task<IActionResult> CrearAgencia([FromBody] CrearAgenciaTransporteDto dto)
        {
            var agencia = new AgenciaTransporte
            {
                Ruc = dto.Ruc.Trim(),
                RazonSocial = dto.RazonSocial.Trim(),
                DireccionAgencia = dto.DireccionAgencia.Trim(),
                Telefono = dto.Telefono,
                Activo = true
            };

            _context.AgenciasTransporte.Add(agencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarAgencias), new { id = agencia.Id }, agencia);
        }
    }
}