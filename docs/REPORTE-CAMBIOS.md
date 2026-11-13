# Texto para actualizar el reporte

## Arquitectura y tecnología — incorporar

El backend de AgroFlet se implementa en C# 14 y ASP.NET Core .NET 10, con Entity Framework Core y MySQL. Sigue la estructura de LearningCenter-main: Domain, Application, Infrastructure e Interfaces por bounded context; servicios CommandServices y QueryServices separados; recursos REST y assemblers; resultados Result<T>; repositorios y UnitOfWork. Las configuraciones de persistencia se organizan mediante ModelBuilderExtensions por contexto. La API publica OpenAPI con Scalar y utiliza JWT con revocación de sesión y autorización por rol y participación.

## Programación orientada a servicios y eventos — incorporar

La solución es una API REST modular con servicios de aplicación y eventos de dominio en proceso. Los agregados generan eventos inmutables con identificadores tipados y los handlers separan historial, efectos de flota, actualización de ETA y notificaciones. DispatchDomainEventsInterceptor se ejecuta en SavingChangesAsync y publica los eventos antes del commit, dentro de la transacción. Si la operación falla, se revierten sus efectos persistentes. Las facades aportan validaciones y proyecciones síncronas entre contextos. No se afirma uso de un broker distribuido ni publicación externa garantizada después del commit.

## API externa — reemplazar

Se implementó el adaptador de Mapbox Directions v5, con perfil mapbox/driving, para consultar geometría GeoJSON, distancia y duración de rutas viales. La consulta se limita a participantes autorizados del envío. Sin credenciales o ante indisponibilidad, la API devuelve que no existe una ruta disponible, sin sustituirla por una ruta ficticia. Las posiciones reportadas se almacenan con fecha y fuente; no constituyen GPS continuo. La llamada autenticada real a Mapbox requiere configurar un token propio y queda pendiente de validación externa.

## Contratos — corregir

GET /shipments devuelve una página con data, totalRecords, currentPage, pageSize y totalPages. PATCH /shipments/{id}/status exige expectedVersion y motivo al cancelar. La cancelación conserva el registro y su historial, y libera recursos. No se implementa DELETE físico de envíos: el DELETE compensatorio del prototipo se reemplaza por una transacción del backend. PUT y PATCH actualizan los recursos según contrato; UPDATE no es un verbo HTTP.

Los compradores consultan proyecciones limitadas de flota vinculada para el detalle autorizado, sin DNI del conductor. Corregir criterios que prohíban toda lectura de flota al comprador si se mantiene ese detalle. Los idiomas persistidos son en/es y se aceptan los alias en_US/es_419.

## Evidencias y alcance — actualizar con VALIDACION.md

Esta entrega contiene exclusivamente backend local para Rider y MySQL. No se desplegó ni se modificó el frontend. Los resultados ejecutados y los límites se consignan en VALIDACION.md; no debe confundirse compilación, pruebas del backend y aceptación E2E. SMTP requiere credenciales y no se ha demostrado envío externo real. Las notificaciones implementadas son internas; FCM, pagos y GPS continuo no se presentan como funcionalidad realizada.

POST /contact-messages persiste las consultas, limita solicitudes y entrega un comprobante, sin publicar un listado anónimo. El reporte contiene menciones a US28 tanto pendiente como finalizada: unificar el estado con evidencia real del formulario y no inferir aceptación completa solo de la existencia de esta API.

## Historias

La matriz TRAZABILIDAD-AV2.md relaciona TS01–TS27 con sus US. No se encontró una selección explícita etiquetada AV2 en el capítulo revisado; se documenta soporte backend, no aceptación oficial del docente ni fechas/commits/despliegues no ejecutados.
