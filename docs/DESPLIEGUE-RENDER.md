# Despliegue de AgroFlet en Render

1. Prepara una base MySQL de demostración accesible desde Render. Crea una base vacía y una cuenta dedicada con permisos sobre esa base. Usa la configuración TLS que indique el proveedor.
2. En Render crea un Web Service vinculado a AGROFLET/Agroflet-backend-application. Selecciona main, runtime Docker y Dockerfile en la raíz. El contenedor escucha en el puerto 10000.
3. Configura estas variables en el servicio:

   - ASPNETCORE_ENVIRONMENT=Production
   - ConnectionStrings__DefaultConnection: cadena MySQL con host remoto, puerto, base y cuenta dedicada.
   - Jwt__Key: secreto aleatorio de al menos 32 bytes.
   - Database__ApplyMigrations=true: aplica las migraciones al iniciar sobre la base de demostración.
   - Cors__AllowedOrigins__0: origen HTTPS del frontend, si lo conectas.
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
