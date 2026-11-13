# Validación de agroflet_backend1

Ejecutada el 11 de noviembre de 2026. Windows, SDK .NET 10.0.403, configuración Release.

- Compilación: 0 errores y 0 advertencias.
- SQLite relacional: 19 aprobadas, 0 fallidas, 0 omitidas.
- MySQL 8.0.44: 19 aprobadas, 0 fallidas, 0 omitidas. Base descartable agroflet_tests_learningcenter20261111, instancia loopback puerto 3311.
- La validación original empleó una migración nueva; queda sustituida por la cadena compatible descrita a continuación.
- Arranque real desde la carpeta final: /health/ready 200, /scalar/v1 200, OpenAPI 42 rutas. Se usó un puerto temporal 5092 y una base de comprobación independiente; el perfil entregado usa 5080.
- Casos verificados: registro/login, permisos y privacidad, JWT y revocación, recuperación, capacidad/reservas, concurrencia, historial, rollback de handlers, ETA e incidentes idempotentes, posiciones, paginación, contacto y respuestas controladas de Mapbox.
- Se añadieron pruebas de autorización previa a contactar el proveedor, Result de validación sin persistencia y cancelación con preservación de historial y privacidad del conductor.

Los TRX y el resultado de arranque están en evidence. Las rutas antiguas que figuran en los TRX corresponden a la carpeta provisional antes del renombrado solicitado; los archivos de código se trasladaron sin alteraciones funcionales y el arranque se volvió a verificar en agroflet_backend1.

No ejecutado: Mapbox con token propio, envío SMTP externo, despliegue ni conexión a la base habitual del usuario. Ningún frontend fue modificado en esta reconstrucción. Los procesos temporales de API/MySQL se apagaron al concluir. La contraseña MySQL del usuario se configura localmente con configurar-local.ps1; no se inventó ni se incluyó una contraseña personal.

## Actualización de compatibilidad (12 de noviembre de 2026)

- Se restauró la cadena 20261019220925_InitialCreate → 20261108235846_AlignAv2Contracts, adaptando namespaces. El modelo final coincide con el actual, sin diferencias pendientes.
- MySQL 26.10.0 aislado en localhost:3311: 20 pruebas aprobadas, 0 fallidas. Incluye migrar desde la versión antigua, conservar un registro existente, comprobar las nuevas tablas y event_id, y repetir la actualización sin efectos adicionales.
- Compilación sin errores. NuGet emitió NU1900 porque no pudo consultar su servicio de vulnerabilidades; no se afirma auditoría de dependencias completada.
- La autenticación real en localhost:3306 fue rechazada (1045). No se aplicaron migraciones ni cambios en la base personal. El usuario informa que Workbench conecta; debe introducir la credencial directamente en su configuración local para resolver la diferencia.
- Evidencia actual: evidence/mysql-upgrade.trx. Las comprobaciones anteriores son históricas.
