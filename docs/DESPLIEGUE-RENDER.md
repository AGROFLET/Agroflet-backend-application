# Despliegue de AgroFlet en Render

1. Prepara una base MySQL de demostración accesible desde Render. Crea una base vacía y una cuenta dedicada con permisos sobre esa base. Usa la configuración TLS que indique el proveedor.
2. En Render crea un Web Service vinculado a AGROFLET/Agroflet-backend-application. Selecciona main, runtime Docker y Dockerfile en la raíz. El contenedor escucha en el puerto de la variable `PORT` que inyecta Render (10000 por defecto).
3. Configura estas variables en el servicio:

   - ASPNETCORE_ENVIRONMENT=Production
   - ConnectionStrings__DefaultConnection: cadena MySQL con host remoto, puerto, base y cuenta dedicada.
   - Jwt__Key: secreto aleatorio de al menos 32 bytes.
   - Database__ApplyMigrations=true: aplica las migraciones al iniciar sobre la base de demostración.
   - Cors__AllowedOrigins__0: origen HTTPS del frontend, si lo conectas.
   - PasswordReset__FrontendUrl: URL pública de la pantalla de restablecer contraseña del frontend.
   - Mapbox__AccessToken: token del servidor, si vas a demostrar el cálculo de rutas.

4. Configura /health/ready como Health Check Path. Despliega y comprueba que el servicio está Live y que /health/ready devuelve HTTP 200.
5. Abre /scalar/v1 y /openapi/v1.json en el dominio público de Render. En Scalar, registra una cuenta de prueba, inicia sesión y consulta GET /api/v1/shipments con el JWT.

## Evidencias para el reporte

- Render Live mostrando servicio, rama y commit desplegado.
- Panel de MySQL mostrando la instancia activa, con las credenciales ocultas.
- Scalar con el dominio público visible, mostrando IAM, Fleet y Shipments.
- Respuesta 200 de /health/ready y de un endpoint protegido. Oculta el encabezado Authorization.
- Si se verifica el frontend, captura el flujo integrado y registra el enlace y la duración real del video.

Agrega al README y al reporte las URL reales del repositorio, Scalar público y OpenAPI público una vez comprobadas.

## Cómo tomar las capturas en Windows

1. En Render, abre el servicio y entra en Events. Abre el último despliegue exitoso y captura el estado Live, la rama y el commit.
2. Desde la URL pública del servicio, abre /scalar/v1. Expande Shipments y captura el dominio en la barra de direcciones y las operaciones visibles.
3. Abre /health/ready. En DevTools > Network, recarga y captura la solicitud con HTTP 200.
4. En Scalar, inicia sesión con la cuenta de demostración y prueba GET /api/v1/shipments. Cierra Request Headers y oculta el campo Bearer Token antes de capturar el estado HTTP y el cuerpo de la respuesta.
5. Usa Win + Shift + S, selecciona el área y guarda las capturas como render-live.png, scalar-publico.png, health-ready.png y shipments-200.png. Si documentas la base, captura su estado desde el proveedor y oculta las credenciales.

## Blueprint gratuito para la demostración

El archivo `render.yaml` configura un Web Service Docker con plan `free`,
puerto 10000 y comprobación `/health/ready`. Render genera la clave JWT;
la conexión MySQL se introduce como secreto al crear el Blueprint.

1. Verifica el correo de tu cuenta de Render.
2. Crea una base MySQL remota vacía de demostración. La base de Workbench en
   `localhost:3306` no es accesible desde Render y no se modifica.
3. En Render, crea un Blueprint desde este repositorio y la rama `main`.
4. Introduce `ConnectionStrings__DefaultConnection` con los datos del proveedor.
   Para un proveedor con cinco conexiones concurrentes, limita el pool a cuatro.
   Ejemplo sin credenciales reales:

   `Server=HOST;Port=PUERTO;Database=BASE;User=USUARIO;Password=SECRETO;SslMode=Required;MaximumPoolSize=4;`

   Usa la validación de certificados que indique el proveedor. No desactives TLS.
5. Revisa que el servicio muestre el plan Free antes de crearlo. Las migraciones
   se aplican al iniciar y necesitan permisos sobre la base de demostración.
6. Verifica `/health/ready`, `/scalar/v1`, `/openapi/v1.json` y una consulta autenticada.

## Dónde alojar MySQL

Render no ofrece MySQL gestionado. Opciones evaluadas (noviembre de 2026):

| Opción | Coste | Notas |
| --- | --- | --- |
| TiDB Cloud Starter (recomendada) | Gratis, sin tarjeta | Compatible con MySQL, 5 GiB y 50 M RU al mes, TLS obligatorio, puerto 4000. |
| Aiven for MySQL Developer | 5 USD/mes | MySQL real. Aiven ya no permite crear MySQL en el plan gratuito. |
| Railway MySQL | Crédito de prueba y luego plan de pago | MySQL real en contenedor. |
| Servicio privado MySQL en Render con disco | Instancia de pago y disco | Los discos persistentes no existen en el plan Free. |
| Filess | Gratis | 10 MB por base y cinco conexiones: solo para una demo mínima. |

Cadena de ejemplo para TiDB Cloud Starter (sin credenciales reales):

`Server=gateway01.REGION.prod.aws.tidbcloud.com;Port=4000;Database=agroflet;User=PREFIJO.root;Password=SECRETO;SslMode=VerifyFull;`

Crea antes la base `agroflet` desde la consola SQL de TiDB (`CREATE DATABASE agroflet;`).

Esta configuración no acredita un despliegue completado. Registra la URL pública
solo después de verificar el servicio y su base remota.

Referencias: https://render.com/docs/blueprint-spec,
https://render.com/docs/free, https://docs.filess.io/es/docs/pricing/plans/,
https://support.filess.io/hc/wiki/en/categories/faq.
