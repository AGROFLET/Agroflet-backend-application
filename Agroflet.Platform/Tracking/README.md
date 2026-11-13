# Tracking e Incidents: actividad durante el transporte

## Fronteras del módulo

Tracking persiste posiciones reportadas y consulta rutas planificadas.
Incidents, ubicado en `../Incidents`, registra incidencias y notificaciones.
Los servicios coordinan con Shipments mediante `IShipmentsContextFacade` y
persisten operaciones con `IUnitOfWork`.

## Endpoints

- `/api/v1/positions`: GET y POST; `/{id}` admite GET.
- `/api/v1/shipments/{id}/planned-route`: GET de ruta planificada.
- `/api/v1/incidents`: GET y POST; `/{id}` admite GET.
- `/api/v1/notifications`: GET; `/{id}` admite GET y PATCH.
- `/api/v1/notifications/{id}/read`: PATCH para marcar la lectura.

`ShipmentActivityController`, en Shipments, ofrece posiciones e incidentes
anidados bajo un envío y la consulta `active/locations`. Los controladores
requieren autenticación. Consultar Resources en Scalar para los cuerpos JSON.

## Idempotencia y autorización

`ReportedPositionCommandService` exige dispatcher y propiedad del envío.
Una posición nueva requiere un envío en tránsito. La pareja ShipmentId/EventId
permite reconocer reintentos: si cambian coordenadas, fecha, origen, nota o actor,
responde 409 `idempotency-conflict`. La fecha se normaliza a UTC al comparar.

`IncidentCommandService` también exige dispatcher y propiedad. Un reintento con
la misma clave devuelve la incidencia existente; reutilizarla cambiando tipo,
descripción, demora o fecha produce 409 `idempotency-key-reused`. La creación
nueva exige tránsito y registra un evento de dominio en una transacción.

## Límites y comprobación propuesta

Las posiciones son reportes, no una conexión GPS continua. `active/locations`
indica ausencia de posición y considera antigua una posición de más de 30 minutos.
El cálculo de ruta depende del proveedor Mapbox y de su token de servidor;
no debe presentarse como verificado solo porque la API compila.

Probar un reporte válido, un reintento idéntico, una clave repetida con datos
distintos y un envío de otro propietario. Repetir con incidencias y comprobar
que un reintento no genera una segunda demora. Son casos propuestos para la
verificación; no se atribuye aquí una ejecución contra servicios remotos.
