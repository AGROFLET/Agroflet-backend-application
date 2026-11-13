# AgroFlet Platform — reconstrucción Learning Center

Backend C# independiente para Rider y MySQL, reconstruido en una solución nueva a partir de la organización del ZIP LearningCenter-main del profesor. Los casos de uso y contratos validados de AgroFlet se portaron y reorganizaron; no se afirma que todas las reglas se hayan inventado nuevamente. Las carpetas anteriores se conservan sin sobrescribir. No contiene frontend, Docker, workflows ni despliegue.

## Ejecutar en Rider, paso a paso

1. Inicia MySQL y abre su conexión en Workbench. Para una instalación nueva ejecuta `database/crear-base.sql` con una cuenta autorizada. Para actualizar `agroflet`, conserva su historial y respalda primero la base: la cadena reconoce `20261019220925_InitialCreate` y agrega `AlignAv2Contracts` sin recrear las tablas existentes. Confirma que estás conectado a la misma instalación de MySQL que contiene esa base.
2. Abre **agroflet_backend1.sln** en Rider. Requiere SDK .NET 10. Si no lo detecta, configura Settings → Build, Execution, Deployment → Toolset and Build → .NET CLI executable path con `C:\Disco Local Toph\App Web\TB1\.tools\dotnet\dotnet.exe`.
3. Abre la terminal PowerShell en la raíz de esta solución y ejecuta:

   ```powershell
   .\configurar-local.ps1 -Usuario root -Puerto 3306
   ```

   Introduce tu contraseña real de MySQL cuando la solicite. Cambia usuario y puerto si utilizas otros. El script crea `Agroflet.Platform/appsettings.Local.json` con la conexión y una clave JWT aleatoria. No sobrescribe configuraciones existentes. El archivo local está excluido de Git y no debe compartirse.
4. Espera la restauración NuGet, selecciona **Agroflet.Platform**, perfil **http**, y pulsa Run. Se aplican las migraciones automáticamente y se abre Scalar.
5. Comprueba `http://localhost:5080/health/ready` y abre `http://localhost:5080/scalar/v1`. La especificación está en `/openapi/v1.json`.
6. En Scalar utiliza `POST /api/v1/auth/register` con los campos del esquema; `userType` admite dispatcher o buyer. Inicia sesión en `/auth/login` y utiliza el accessToken en Authorize para probar recursos protegidos. También puedes ejecutar el archivo `Agroflet.Platform.http` en Rider. Sus usuarios son ejemplos ficticios, no cuentas preinstaladas.

Si Rider no encuentra dotnet, selecciona el SDK indicado. Si MySQL responde Access denied, corrige usuario, contraseña o permisos en el archivo local. Si la conexión es rechazada, comprueba el servicio y puerto. Las migraciones requieren permisos para crear y modificar tablas en la base seleccionada. El error 1045 indica credenciales rechazadas; el 1044 indica permisos insuficientes sobre la base. No borres el historial de migraciones para resolverlos.

## Organización, igual al patrón del ejemplo

```text
Agroflet.Platform/
  Iam/ Fleet/ Shipments/ Tracking/ Incidents/ Contact/
    Domain/Model/                 Agregados, Commands, Queries, Events
    Domain/Repositories/          Contratos de persistencia
    Application/CommandServices/  Contratos de escritura
    Application/QueryServices/    Contratos de consulta
    Application/Internal/         Implementaciones y servicios salientes
    Infrastructure/Persistence/EntityFrameworkCore/
    Interfaces/Rest/              Controllers, Resources, Transform
  Shared/                         Result, eventos, UnitOfWork, errores
  Migrations/                     Cadena compatible y snapshot
Agroflet.Platform.Tests/
```

Resource → assembler → Command/Query → servicio Handle → dominio/repositorio → UnitOfWork/MySQL. Los resultados esperados usan Result<T>; los assemblers REST los traducen a HTTP. Las composiciones de varias consultas propagan el primer error. Las validaciones del dominio se conservan y los fallos inesperados se manejan centralmente.

Ver `docs/ALINEACION-LEARNING-CENTER.md` para coincidencias y adaptaciones justificadas; `docs/TRAZABILIDAD-AV2.md` para TS01–TS27; y `docs/REPORTE-CAMBIOS.md` para texto listo para incorporar al reporte.

## API externa

**Mapbox Directions v5**, perfil mapbox/driving. Configura `Mapbox:AccessToken` en appsettings.Local.json, exclusivamente en el servidor. GET `/api/v1/shipments/{id}/planned-route` devuelve ruta GeoJSON, distancia y duración a participantes autorizados. Sin token devuelve indisponibilidad; no inventa rutas. No se ha probado una llamada autenticada real. Fuente: https://docs.mapbox.com/api/navigation/directions/ . El perfil no acredita restricciones de camiones y las posiciones reportadas no son GPS continuo.

La recuperación de contraseña utiliza SMTP configurable. En Development puede devolver un token para pruebas locales conforme a appsettings.Development.json. Para correo real configura Smtp y desactiva PasswordReset:ReturnTokenInDevelopment. No se ha verificado entrega SMTP externa. No necesitas Mapbox ni SMTP para iniciar el backend y probar las demás operaciones.

## HTTP y alcance

Se implementan GET, POST, PATCH y PUT según las historias. UPDATE es una operación de persistencia, no un verbo HTTP. La cancelación es PATCH con motivo y conserva historial; no hay DELETE físico de envíos. Las escrituras parciales se revierten transaccionalmente, por lo que no se necesita el DELETE compensatorio del mock. Las notificaciones son internas; no se implementan pagos, FCM ni telemetría automática.

## Pruebas

`dotnet test agroflet_backend1.sln -c Release -m:1` utiliza SQLite aislado. Para MySQL, TEST_MYSQL_CONNECTION debe apuntar exclusivamente a una base descartable `agroflet_tests_<sufijo>`: el fixture la elimina y reconstruye. Nunca usar una base personal en esa variable. Los resultados ejecutados se detallan en `docs/VALIDACION.md`.

La compatibilidad aquí significa contratos de datos y permisos; no implica que el frontend de demostración pueda conectarse cambiando solamente la URL. El frontend no se ha modificado en esta reconstrucción. El despliegue queda fuera de esta entrega.

## Actualizar credenciales locales

Si el archivo local ya existe, ejecuta en PowerShell dentro de esta carpeta:

    .\configurar-local.ps1 -Actualizar -Usuario root -Servidor 127.0.0.1 -Puerto 3306

Introduce la contraseña directamente en la terminal. Conserva la base elegida, JWT, Mapbox y las demás opciones locales. Para cambiar también la base añade -BaseDeDatos agroflet (actualización de la base anterior) o -BaseDeDatos agroflet_learning_center (instalación independiente). Después ejecuta el perfil http en Rider y comprueba /health/ready. Workbench y el backend deben usar el mismo servidor, puerto y usuario.

Para publicar la API con Scalar, consulta [Despliegue en Render](docs/DESPLIEGUE-RENDER.md).
