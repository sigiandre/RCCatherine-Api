using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RCCatherineApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgenciasTransporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ruc = table.Column<string>(type: "text", nullable: false),
                    RazonSocial = table.Column<string>(type: "text", nullable: false),
                    DireccionAgencia = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenciasTransporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoDocumento = table.Column<string>(type: "text", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "text", nullable: false),
                    NombreRazonSocial = table.Column<string>(type: "text", nullable: false),
                    DireccionFiscal = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Correo = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ruc = table.Column<string>(type: "text", nullable: false),
                    RazonSocial = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Contacto = table.Column<string>(type: "text", nullable: true),
                    EsInternacional = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Dni = table.Column<string>(type: "text", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesMedidaSunat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoSunat = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedidaSunat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DireccionesCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    Etiqueta = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: false),
                    Ubigeo = table.Column<string>(type: "text", nullable: true),
                    Referencia = table.Column<string>(type: "text", nullable: true),
                    EsDireccionFiscal = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DireccionesCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DireccionesCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "text", nullable: false),
                    TipoIngreso = table.Column<string>(type: "text", nullable: false),
                    ProveedorId = table.Column<int>(type: "integer", nullable: false),
                    TrabajadorId = table.Column<int>(type: "integer", nullable: false),
                    FleteInternacional = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Seguro = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Aranceles = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    GastosDesaduanaje = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SubtotalFob = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCostosLogisticos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCostoPuestoAlmacen = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Justificacion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingresos_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingresos_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    UnidadBaseId = table.Column<int>(type: "integer", nullable: false),
                    PrecioMenor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioMayor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CostoPromedioPonderado = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    StockFisico = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    StockReservado = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_UnidadesMedidaSunat_UnidadBaseId",
                        column: x => x.UnidadBaseId,
                        principalTable: "UnidadesMedidaSunat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiasVigencia = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    DireccionEntregaId = table.Column<int>(type: "integer", nullable: true),
                    VendedorId = table.Column<int>(type: "integer", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Igv = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_DireccionesCliente_DireccionEntregaId",
                        column: x => x.DireccionEntregaId,
                        principalTable: "DireccionesCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Trabajadores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesIngreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IngresoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedidaSunatId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FactorConversion = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoUnitarioFob = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoLogisticoProrrateado = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoRealUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesIngreso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesIngreso_Ingresos_IngresoId",
                        column: x => x.IngresoId,
                        principalTable: "Ingresos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesIngreso_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesIngreso_UnidadesMedidaSunat_UnidadMedidaSunatId",
                        column: x => x.UnidadMedidaSunatId,
                        principalTable: "UnidadesMedidaSunat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquivalenciasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedidaSunatId = table.Column<int>(type: "integer", nullable: false),
                    FactorUnidadesBase = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquivalenciasProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquivalenciasProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquivalenciasProducto_UnidadesMedidaSunat_UnidadMedidaSunat~",
                        column: x => x.UnidadMedidaSunatId,
                        principalTable: "UnidadesMedidaSunat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosKardex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    TrabajadorId = table.Column<int>(type: "integer", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "text", nullable: false),
                    ReferenciaDocumento = table.Column<string>(type: "text", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    CantidadUnidadesBase = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoPromedioResultante = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoCantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoValorizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosKardex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosKardex_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosKardex_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCotizacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CotizacionId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedidaSunatId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FactorConversion = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    TipoListaPrecio = table.Column<string>(type: "text", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCotizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesCotizacion_Cotizaciones_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCotizacion_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCotizacion_UnidadesMedidaSunat_UnidadMedidaSunatId",
                        column: x => x.UnidadMedidaSunatId,
                        principalTable: "UnidadesMedidaSunat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroOrden = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CotizacionId = table.Column<int>(type: "integer", nullable: true),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    DireccionEntregaId = table.Column<int>(type: "integer", nullable: true),
                    VendedorId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Igv = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MontoPagado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoPendiente = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NumeroGuiaAgencia = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesVenta_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesVenta_Cotizaciones_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesVenta_DireccionesCliente_DireccionEntregaId",
                        column: x => x.DireccionEntregaId,
                        principalTable: "DireccionesCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesVenta_Trabajadores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesElectronicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdenVentaId = table.Column<int>(type: "integer", nullable: false),
                    TipoComprobante = table.Column<string>(type: "text", nullable: false),
                    Serie = table.Column<string>(type: "text", nullable: false),
                    Correlativo = table.Column<string>(type: "text", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoDocumentoCliente = table.Column<string>(type: "text", nullable: false),
                    NumeroDocumentoCliente = table.Column<string>(type: "text", nullable: false),
                    NombreRazonSocialCliente = table.Column<string>(type: "text", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Igv = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoDeudor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    EstadoSunat = table.Column<string>(type: "text", nullable: false),
                    NombreArchivoXml = table.Column<string>(type: "text", nullable: true),
                    CodigoCdr = table.Column<string>(type: "text", nullable: true),
                    DescripcionCdr = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesElectronicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesElectronicos_OrdenesVenta_OrdenVentaId",
                        column: x => x.OrdenVentaId,
                        principalTable: "OrdenesVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesOrdenVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdenVentaId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedidaSunatId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FactorConversion = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    UnidadesBaseReservadas = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesOrdenVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesOrdenVenta_OrdenesVenta_OrdenVentaId",
                        column: x => x.OrdenVentaId,
                        principalTable: "OrdenesVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesOrdenVenta_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesOrdenVenta_UnidadesMedidaSunat_UnidadMedidaSunatId",
                        column: x => x.UnidadMedidaSunatId,
                        principalTable: "UnidadesMedidaSunat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuiasRemision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdenVentaId = table.Column<int>(type: "integer", nullable: false),
                    Serie = table.Column<string>(type: "text", nullable: false),
                    Correlativo = table.Column<string>(type: "text", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaTraslado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PuntoPartida = table.Column<string>(type: "text", nullable: false),
                    PuntoLlegada = table.Column<string>(type: "text", nullable: false),
                    MotivoTraslado = table.Column<string>(type: "text", nullable: false),
                    NumeroBultos = table.Column<int>(type: "integer", nullable: false),
                    PesoBrutoTotalKg = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    ModalidadTransporte = table.Column<string>(type: "text", nullable: false),
                    AgenciaTransporteId = table.Column<int>(type: "integer", nullable: true),
                    PlacaVehiculo = table.Column<string>(type: "text", nullable: true),
                    LicenciaConductor = table.Column<string>(type: "text", nullable: true),
                    DniConductor = table.Column<string>(type: "text", nullable: true),
                    NombresApellidosConductor = table.Column<string>(type: "text", nullable: true),
                    EstadoSunat = table.Column<string>(type: "text", nullable: false),
                    CodigoCdr = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiasRemision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuiasRemision_AgenciasTransporte_AgenciaTransporteId",
                        column: x => x.AgenciaTransporteId,
                        principalTable: "AgenciasTransporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuiasRemision_OrdenesVenta_OrdenVentaId",
                        column: x => x.OrdenVentaId,
                        principalTable: "OrdenesVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagosVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdenVentaId = table.Column<int>(type: "integer", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodoPago = table.Column<string>(type: "text", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NumeroOperacion = table.Column<string>(type: "text", nullable: true),
                    RegistradoPorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosVenta_OrdenesVenta_OrdenVentaId",
                        column: x => x.OrdenVentaId,
                        principalTable: "OrdenesVenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagosVenta_Trabajadores_RegistradoPorId",
                        column: x => x.RegistradoPorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotasCreditoDebito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComprobanteElectronicoId = table.Column<int>(type: "integer", nullable: false),
                    TipoNota = table.Column<string>(type: "text", nullable: false),
                    Serie = table.Column<string>(type: "text", nullable: false),
                    Correlativo = table.Column<string>(type: "text", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CodigoMotivoSunat = table.Column<string>(type: "text", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExtornaMercaderiaKardex = table.Column<bool>(type: "boolean", nullable: false),
                    EstadoSunat = table.Column<string>(type: "text", nullable: false),
                    CodigoCdr = table.Column<string>(type: "text", nullable: true),
                    DescripcionCdr = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasCreditoDebito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasCreditoDebito_ComprobantesElectronicos_ComprobanteElec~",
                        column: x => x.ComprobanteElectronicoId,
                        principalTable: "ComprobantesElectronicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_NumeroDocumento",
                table: "Clientes",
                column: "NumeroDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesElectronicos_OrdenVentaId",
                table: "ComprobantesElectronicos",
                column: "OrdenVentaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_ClienteId",
                table: "Cotizaciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_Codigo",
                table: "Cotizaciones",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_DireccionEntregaId",
                table: "Cotizaciones",
                column: "DireccionEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_VendedorId",
                table: "Cotizaciones",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCotizacion_CotizacionId",
                table: "DetallesCotizacion",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCotizacion_ProductoId",
                table: "DetallesCotizacion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCotizacion_UnidadMedidaSunatId",
                table: "DetallesCotizacion",
                column: "UnidadMedidaSunatId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesIngreso_IngresoId",
                table: "DetallesIngreso",
                column: "IngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesIngreso_ProductoId",
                table: "DetallesIngreso",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesIngreso_UnidadMedidaSunatId",
                table: "DetallesIngreso",
                column: "UnidadMedidaSunatId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesOrdenVenta_OrdenVentaId",
                table: "DetallesOrdenVenta",
                column: "OrdenVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesOrdenVenta_ProductoId",
                table: "DetallesOrdenVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesOrdenVenta_UnidadMedidaSunatId",
                table: "DetallesOrdenVenta",
                column: "UnidadMedidaSunatId");

            migrationBuilder.CreateIndex(
                name: "IX_DireccionesCliente_ClienteId",
                table: "DireccionesCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_EquivalenciasProducto_ProductoId",
                table: "EquivalenciasProducto",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_EquivalenciasProducto_UnidadMedidaSunatId",
                table: "EquivalenciasProducto",
                column: "UnidadMedidaSunatId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiasRemision_AgenciaTransporteId",
                table: "GuiasRemision",
                column: "AgenciaTransporteId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiasRemision_OrdenVentaId",
                table: "GuiasRemision",
                column: "OrdenVentaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_ProveedorId",
                table: "Ingresos",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_TrabajadorId",
                table: "Ingresos",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosKardex_ProductoId",
                table: "MovimientosKardex",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosKardex_TrabajadorId",
                table: "MovimientosKardex",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasCreditoDebito_ComprobanteElectronicoId",
                table: "NotasCreditoDebito",
                column: "ComprobanteElectronicoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesVenta_ClienteId",
                table: "OrdenesVenta",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesVenta_CotizacionId",
                table: "OrdenesVenta",
                column: "CotizacionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesVenta_DireccionEntregaId",
                table: "OrdenesVenta",
                column: "DireccionEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesVenta_NumeroOrden",
                table: "OrdenesVenta",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesVenta_VendedorId",
                table: "OrdenesVenta",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosVenta_OrdenVentaId",
                table: "PagosVenta",
                column: "OrdenVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosVenta_RegistradoPorId",
                table: "PagosVenta",
                column: "RegistradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadBaseId",
                table: "Productos",
                column: "UnidadBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_Usuario",
                table: "Trabajadores",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesMedidaSunat_CodigoSunat",
                table: "UnidadesMedidaSunat",
                column: "CodigoSunat",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesCotizacion");

            migrationBuilder.DropTable(
                name: "DetallesIngreso");

            migrationBuilder.DropTable(
                name: "DetallesOrdenVenta");

            migrationBuilder.DropTable(
                name: "EquivalenciasProducto");

            migrationBuilder.DropTable(
                name: "GuiasRemision");

            migrationBuilder.DropTable(
                name: "MovimientosKardex");

            migrationBuilder.DropTable(
                name: "NotasCreditoDebito");

            migrationBuilder.DropTable(
                name: "PagosVenta");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropTable(
                name: "AgenciasTransporte");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "ComprobantesElectronicos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "UnidadesMedidaSunat");

            migrationBuilder.DropTable(
                name: "OrdenesVenta");

            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "DireccionesCliente");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
