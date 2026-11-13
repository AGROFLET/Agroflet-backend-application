# Shipments: envíos y persistencia

## Responsabilidades

El contexto modela envíos, ubicaciones e historial de estados. Los controladores
usan assemblers, servicios de aplicación y repositorios; `Shipment` concentra
las invariantes y emite eventos de ciclo de vida y cambio de llegada estimada.
Las migraciones compartidas se encuentran en `../Migrations` y el script inicial
de base en `../../database/crear-base.sql`.

## Contratos principales

- `GET /api/v1/shipments` devuelve `data`, `totalRecords`, `currentPage`,
  `pageSize` y `totalPages`. `legacy=true` devuelve la colección anterior.
- `POST /api/v1/shipments` crea un envío.
- `GET /api/v1/shipments/{id}` consulta un envío.
- `PATCH /api/v1/shipments/{id}` actualiza el envío.
- `PATCH /api/v1/shipments/{id}/status` exige `status` y `expectedVersion`.
- `/api/v1/locations` y `/api/v1/status-changes` ofrecen GET de colección y por ID.

Los controladores requieren autenticación. La actividad anidada del envío
coordina servicios de Tracking e Incidents; no duplica sus entidades de dominio.

## Invariantes del agregado

El peso debe ser positivo; origen y destino deben existir como identificadores
positivos y ser distintos. La llegada estimada debe ser posterior a la salida
planificada. Se admiten los estados `planned`, `in_transit`, `delivered` y
`cancelled`, pero las transiciones se validan mediante `ShipmentStatus`.

El código `AGF-` se asigna después de obtener un ID persistido. Cancelar exige
un motivo y conserva fechas e historial: no equivale a eliminar el envío.
Un incidente solo puede retrasar un envío en tránsito y genera un evento de ETA.

## Persistencia y comprobación

No borrar `__EFMigrationsHistory` para forzar una actualización. Respaldar la
base existente antes de aplicar migraciones y no ejecutar fixtures destructivos
de pruebas sobre datos personales. La guía completa está en `../../docs`.

Casos de verificación propuestos: creación válida, peso cero, origen igual al
destino, llegada anterior a salida, transición inválida y cancelación sin motivo.
Comparar el listado paginado con `legacy=true` usando la misma identidad.
Esta documentación no afirma haber ejecutado esos casos contra Render.
