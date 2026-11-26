# RCCatherine API

Backend RESTful desarrollado en **ASP.NET Core** con **PostgreSQL** para la gestión comercial y operativa de **Representaciones Catherine**.

## Módulos y flujo principal

* **Órdenes de venta (`OrdenesVenta`):** creación de órdenes directas o desde cotización aprobada, cálculo de precios según lista (`MENOR`, `MAYOR`, `PERSONALIZADO`), reserva de stock y control de saldo pendiente (`RESERVADA`).
* **Facturación y pagos (`ComprobantesElectronicos` / `Pagos`):** emisión de comprobantes electrónicos (`BOLETA` / `FACTURA`), transición de estado a `FACTURADA` y registro de pagos (`EFECTIVO`, `YAPE`, `PLIN`, `TRANSFERENCIA`) actualizando montos pagados y saldos.
* **Despacho y logística (`GuiasRemision`):** gestión de órdenes listas para despacho, emisión de guías de remisión, registro de tracking de agencia y cierre a `DESPACHADA`.

## Arquitectura y reglas clave

* **Capa de negocio:** centralizada en servicios de dominio (`VentaFacturacionService` y controladores asociados) para garantizar consistencia transaccional en PostgreSQL.
* **Integridad financiera:** control explícito de `MontoPagado` y `SaldoPendiente` sin valores nulos inconsistentes.
* **Autenticación:** endpoints protegidos mediante JWT y resolución del trabajador autenticado desde el token.

## Ejecución local

1. Configura la cadena de conexión a **PostgreSQL** y los parámetros JWT en tu entorno (`appsettings.Development.json` o variables de entorno, sin exponer secretos en el repositorio).
2. Restaura paquetes y ejecuta la API:

```bash
dotnet restore
dotnet run
