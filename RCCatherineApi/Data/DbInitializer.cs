using Microsoft.EntityFrameworkCore;
using RCCatherineApi.Models;

namespace RCCatherineApi.Data
{
    public static class DbInitializer
    {
        public static async Task InicializarAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            // Catálogo N° 03 SUNAT (REQ-INV-01)
            if (!await context.UnidadesMedidaSunat.AnyAsync())
            {
                context.UnidadesMedidaSunat.AddRange(
                    new UnidadMedidaSunat { CodigoSunat = "NIU", Descripcion = "Unidad (Bienes)", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "BX", Descripcion = "Caja", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "PK", Descripcion = "Paquete", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "BG", Descripcion = "Bolsa", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "DZN", Descripcion = "Docena", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "GRO", Descripcion = "Gruesa", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "MLL", Descripcion = "Millares", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "KGM", Descripcion = "Kilogramo", Activo = true },
                    new UnidadMedidaSunat { CodigoSunat = "CJ", Descripcion = "Conos", Activo = true }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Categorias.AnyAsync())
            {
                context.Categorias.AddRange(
                    new Categoria { Nombre = "General", Descripcion = "Productos generales", Activo = true },
                    new Categoria { Nombre = "Insumos", Descripcion = "Insumos y materiales", Activo = true }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Trabajadores.AnyAsync())
            {
                context.Trabajadores.Add(new Trabajador
                {
                    NombreCompleto = "Administrador Catherine",
                    Dni = "00000000",
                    Usuario = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                    Rol = "Administrador",
                    Activo = true
                });
                await context.SaveChangesAsync();
            }
        }
    }
}