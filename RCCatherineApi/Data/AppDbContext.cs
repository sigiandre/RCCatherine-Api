using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Models;

namespace RCCatherineApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Trabajador> Trabajadores => Set<Trabajador>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<DireccionCliente> DireccionesCliente => Set<DireccionCliente>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<AgenciaTransporte> AgenciasTransporte => Set<AgenciaTransporte>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<UnidadMedidaSunat> UnidadesMedidaSunat => Set<UnidadMedidaSunat>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<EquivalenciaProducto> EquivalenciasProducto => Set<EquivalenciaProducto>();
        public DbSet<Ingreso> Ingresos => Set<Ingreso>();
        public DbSet<DetalleIngreso> DetallesIngreso => Set<DetalleIngreso>();
        public DbSet<MovimientoKardex> MovimientosKardex => Set<MovimientoKardex>();
        public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
        public DbSet<DetalleCotizacion> DetallesCotizacion => Set<DetalleCotizacion>();
        public DbSet<OrdenVenta> OrdenesVenta => Set<OrdenVenta>();
        public DbSet<DetalleOrdenVenta> DetallesOrdenVenta => Set<DetalleOrdenVenta>();
        public DbSet<PagoVenta> PagosVenta => Set<PagoVenta>();
        public DbSet<ComprobanteElectronico> ComprobantesElectronicos => Set<ComprobanteElectronico>();
        public DbSet<NotaCreditoDebito> NotasCreditoDebito => Set<NotaCreditoDebito>();
        public DbSet<GuiaRemision> GuiasRemision => Set<GuiaRemision>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trabajador>()
                .HasIndex(t => t.Usuario)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.NumeroDocumento);

            modelBuilder.Entity<UnidadMedidaSunat>()
                .HasIndex(u => u.CodigoSunat)
                .IsUnique();

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasIndex(p => p.Codigo).IsUnique();
                entity.Ignore(p => p.StockDisponible);

                entity.Property(p => p.PrecioMenor).HasPrecision(18, 2);
                entity.Property(p => p.PrecioMayor).HasPrecision(18, 2);
                entity.Property(p => p.CostoPromedioPonderado).HasPrecision(18, 4);
                entity.Property(p => p.StockFisico).HasPrecision(18, 4);
                entity.Property(p => p.StockReservado).HasPrecision(18, 4);
                entity.Property(p => p.StockMinimo).HasPrecision(18, 4);

                entity.HasOne(p => p.UnidadBase)
                    .WithMany()
                    .HasForeignKey(p => p.UnidadBaseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EquivalenciaProducto>(entity =>
            {
                entity.Property(e => e.FactorUnidadesBase).HasPrecision(18, 4);

                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.Equivalencias)
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UnidadMedidaSunat)
                    .WithMany(u => u.Equivalencias)
                    .HasForeignKey(e => e.UnidadMedidaSunatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.Property(i => i.FleteInternacional).HasPrecision(18, 2);
                entity.Property(i => i.Seguro).HasPrecision(18, 2);
                entity.Property(i => i.Aranceles).HasPrecision(18, 2);
                entity.Property(i => i.GastosDesaduanaje).HasPrecision(18, 2);
                entity.Property(i => i.SubtotalFob).HasPrecision(18, 2);
                entity.Property(i => i.TotalCostosLogisticos).HasPrecision(18, 2);
                entity.Property(i => i.TotalCostoPuestoAlmacen).HasPrecision(18, 2);

                entity.HasOne(i => i.Trabajador)
                    .WithMany(t => t.Ingresos)
                    .HasForeignKey(i => i.TrabajadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DetalleIngreso>(entity =>
            {
                entity.Property(d => d.Cantidad).HasPrecision(18, 4);
                entity.Property(d => d.FactorConversion).HasPrecision(18, 4);
                entity.Property(d => d.CostoUnitarioFob).HasPrecision(18, 4);
                entity.Property(d => d.CostoLogisticoProrrateado).HasPrecision(18, 4);
                entity.Property(d => d.CostoRealUnitario).HasPrecision(18, 4);
                entity.Property(d => d.Subtotal).HasPrecision(18, 2);

                entity.HasOne(d => d.UnidadMedidaSunat)
                    .WithMany()
                    .HasForeignKey(d => d.UnidadMedidaSunatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MovimientoKardex>(entity =>
            {
                entity.Property(m => m.CantidadUnidadesBase).HasPrecision(18, 4);
                entity.Property(m => m.CostoUnitario).HasPrecision(18, 4);
                entity.Property(m => m.CostoPromedioResultante).HasPrecision(18, 4);
                entity.Property(m => m.SaldoCantidad).HasPrecision(18, 4);
                entity.Property(m => m.SaldoValorizado).HasPrecision(18, 2);

                entity.HasOne(m => m.Trabajador)
                    .WithMany(t => t.MovimientosKardex)
                    .HasForeignKey(m => m.TrabajadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cotizacion>(entity =>
            {
                entity.HasIndex(c => c.Codigo).IsUnique();
                entity.Property(c => c.Subtotal).HasPrecision(18, 2);
                entity.Property(c => c.Igv).HasPrecision(18, 2);
                entity.Property(c => c.Total).HasPrecision(18, 2);

                entity.HasOne(c => c.Vendedor)
                    .WithMany(t => t.Cotizaciones)
                    .HasForeignKey(c => c.VendedorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.DireccionEntrega)
                    .WithMany()
                    .HasForeignKey(c => c.DireccionEntregaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DetalleCotizacion>(entity =>
            {
                entity.Property(d => d.Cantidad).HasPrecision(18, 4);
                entity.Property(d => d.FactorConversion).HasPrecision(18, 4);
                entity.Property(d => d.PrecioUnitario).HasPrecision(18, 2);
                entity.Property(d => d.Subtotal).HasPrecision(18, 2);

                entity.HasOne(d => d.UnidadMedidaSunat)
                    .WithMany()
                    .HasForeignKey(d => d.UnidadMedidaSunatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrdenVenta>(entity =>
            {
                entity.HasIndex(o => o.NumeroOrden).IsUnique();
                entity.Property(o => o.Subtotal).HasPrecision(18, 2);
                entity.Property(o => o.Igv).HasPrecision(18, 2);
                entity.Property(o => o.Total).HasPrecision(18, 2);
                entity.Property(o => o.MontoPagado).HasPrecision(18, 2);
                entity.Property(o => o.SaldoPendiente).HasPrecision(18, 2);

                entity.HasOne(o => o.Cotizacion)
                    .WithOne(c => c.OrdenVenta)
                    .HasForeignKey<OrdenVenta>(o => o.CotizacionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Vendedor)
                    .WithMany(t => t.OrdenesVenta)
                    .HasForeignKey(o => o.VendedorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.DireccionEntrega)
                    .WithMany()
                    .HasForeignKey(o => o.DireccionEntregaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ComprobanteElectronico)
                    .WithOne(c => c.OrdenVenta)
                    .HasForeignKey<ComprobanteElectronico>(c => c.OrdenVentaId);

                entity.HasOne(o => o.GuiaRemision)
                    .WithOne(g => g.OrdenVenta)
                    .HasForeignKey<GuiaRemision>(g => g.OrdenVentaId);
            });

            modelBuilder.Entity<DetalleOrdenVenta>(entity =>
            {
                entity.Property(d => d.Cantidad).HasPrecision(18, 4);
                entity.Property(d => d.FactorConversion).HasPrecision(18, 4);
                entity.Property(d => d.UnidadesBaseReservadas).HasPrecision(18, 4);
                entity.Property(d => d.PrecioUnitario).HasPrecision(18, 2);
                entity.Property(d => d.Subtotal).HasPrecision(18, 2);

                entity.HasOne(d => d.UnidadMedidaSunat)
                    .WithMany()
                    .HasForeignKey(d => d.UnidadMedidaSunatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PagoVenta>(entity =>
            {
                entity.Property(p => p.Monto).HasPrecision(18, 2);
                entity.HasOne(p => p.RegistradoPor)
                    .WithMany()
                    .HasForeignKey(p => p.RegistradoPorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ComprobanteElectronico>(entity =>
            {
                entity.Property(c => c.Subtotal).HasPrecision(18, 2);
                entity.Property(c => c.Igv).HasPrecision(18, 2);
                entity.Property(c => c.Total).HasPrecision(18, 2);
                entity.Property(c => c.SaldoDeudor).HasPrecision(18, 2);
            });

            modelBuilder.Entity<NotaCreditoDebito>(entity =>
            {
                entity.Property(n => n.Monto).HasPrecision(18, 2);
            });

            modelBuilder.Entity<GuiaRemision>(entity =>
            {
                entity.Property(g => g.PesoBrutoTotalKg).HasPrecision(18, 3);
                entity.HasOne(g => g.AgenciaTransporte)
                    .WithMany(a => a.GuiasRemision)
                    .HasForeignKey(g => g.AgenciaTransporteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}