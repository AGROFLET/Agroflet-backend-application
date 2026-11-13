# Fleet: vehículos y conductores

## Organización

`VehiclesController` y `DriversController` reciben Resources. Los assemblers
construyen Commands y los servicios de aplicación coordinan repositorios y
`IUnitOfWork`. `Vehicle` y `Driver` pertenecen al dominio; la persistencia EF Core
está en `Infrastructure/Persistence/EntityFrameworkCore`.

## Contratos HTTP

- `/api/v1/vehicles`: GET de colección y POST de creación.
- `/api/v1/vehicles/{id}`: GET, PATCH y PUT.
- `/api/v1/vehicles/{id}/status`: PATCH con el campo `status`.
- `/api/v1/drivers`: GET de colección y POST de creación.
- `/api/v1/drivers/{id}`: GET y PATCH.

Todos estos controladores requieren autenticación. Los detalles de Resources
se publican en OpenAPI; el módulo no expone DELETE de vehículos o conductores.

## Reglas de vehículos

`VehicleCommandService` exige un dispatcher. Al crear, toma el propietario de
la identidad autenticada y establece `available`, sin confiar en un propietario
proporcionado por el cliente. Al editar, un vehículo ajeno responde 404.
Los estados `reserved` e `in_route` bloquean la edición con 409. La actualización
operativa permite `available` y `under_maintenance`.

`FleetContextFacade` expone la frontera de integración con otros contextos.
`ShipmentFleetEventHandler` conecta el módulo con eventos del ciclo del envío.
La representación de conductores considera si el actor tiene rol dispatcher.

## Verificación manual propuesta

1. Autenticarse como dispatcher y crear un vehículo y un conductor de prueba.
2. Consultarlos por identificador y desde sus colecciones.
3. Editar un vehículo disponible y cambiarlo a mantenimiento.
4. Comprobar el rechazo al editar un vehículo reservado o en ruta.
5. Probar el acceso de otro dispatcher y verificar el aislamiento.

Consultar `/scalar/v1` para los campos obligatorios. Esta lista describe casos
para verificar; no representa resultados de un despliegue remoto.
