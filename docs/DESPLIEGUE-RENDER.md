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
