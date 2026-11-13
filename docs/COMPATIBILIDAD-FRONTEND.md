# Compatibilidad de contratos, sin modificar frontend

El 11 de noviembre se revisaron en AGROFLET/Agroflet-frontend-application los archivos src/iam/infrastructure/iam-api.js, src/shipments/infrastructure/shipments-api.js y src/tracking/infrastructure/tracking-api.js.

El IAM remoto todavía genera tokens de demostración y consulta contraseñas del mock; Shipments todavía contempla DELETE como compensación y creación manual de historial. Por ello la integración futura necesita sustituir esa orquestación por llamadas a la API real. No se puede afirmar conexión completa cambiando solo la URL, y no se alteró ese frontend en esta tarea.

Se mantienen IDs enteros, camelCase, roles dispatcher/buyer, catálogos, aliases de rutas y campos operativos. Para la integración futura: usar /auth/register y /auth/login; enviar Bearer JWT; consumir data y paginación de GET /shipments; enviar expectedVersion al cambiar estado; omitir escrituras de historial/alertas/reservas desde el navegador; usar eventId/idempotencyKey para reintentos. El servidor genera efectos transaccionales y rechaza escrituras públicas de datos sensibles.

El estado y los textos del reporte se actualizan con REPORTE-CAMBIOS.md. El frontend, su publicación y el despliegue del backend quedan fuera de esta entrega.
