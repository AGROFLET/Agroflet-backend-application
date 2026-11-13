# Alineación con LearningCenter-main

Fuente: C:/Users/Usuario/Downloads/LearningCenter-main.zip. SHA256: 08C6B892B75874A2B0474F4A25C7F525FB12E2CAFDC9DA9C1F9151CC3C7A575C. Se revisaron Program, csproj, servicios, Result, AggregateRoot, interceptor, configuración de EF y estructura de Iam/Profiles/Publishing. También se revisó la extracción de learning-center-eventos.pptx.

| Patrón del profesor | Aplicación en AgroFlet |
|---|---|
| .NET 10, C# 14, EF Core, MySQL | .NET 10; EF 10.0.14 y proveedor MySQL 10.0.12 del ZIP |
| Contextos y cuatro capas | Iam, Fleet, Shipments, Tracking, Incidents y Contact |
| Application/CommandServices y QueryServices | Contratos separados y clases Handle en Internal |
| Result<T> y traducción HTTP | Result<T> con Match; errores de negocio y validación explícitos; assembler REST con ProblemDetails localizados |
| Repositories y UnitOfWork | Interfaces en Domain, implementaciones EntityFrameworkCore y transacciones reales |
| ModelBuilderExtensions por contexto | ApplyIam/Fleet/Shipments/Tracking/Incidents/ContactConfiguration |
| IHashingService e ITokenService | BCrypt y JWT separados; CredentialService como fachada de compatibilidad |
| AggregateRoot + IEvent + IEventHandler | Eventos inmutables en agregados y handlers independientes con ILogger |
| DispatchDomainEventsInterceptor | SavingChangesAsync, publicación secuencial, limpieza y procesamiento de eventos derivados |
| Migrations en raíz del proyecto | InitialCreate original + AlignAv2Contracts; admite base vacía y actualización desde la versión anterior |
| Resources y Transform | Entradas/salidas separadas de entidades y assemblers de comandos/recursos |

## Adaptaciones necesarias, sin atribuirlas al profesor

- Se conserva autenticación JwtBearer estándar, validación de versión/revocación y alcance por participante. No se copia el middleware de autorización didáctico ni CORS abierto de la plantilla.
- Los identificadores en eventos son tipos propios (ShipmentId, UserId, VehicleId, DriverId, IncidentId); la persistencia y los contratos HTTP conservan enteros para compatibilidad. No se afirma que todas las propiedades EF sean value objects como en algunos agregados de la plantilla.
- Result<T> traduce fallos esperados mediante ExecuteAsync y un error con código/estado. No se copió literalmente la clase Result del profesor ni se incluyen enums sin uso. Las consultas compuestas utilizan RequireValue para detenerse ante el primer fallo, conservando ProblemDetails centralizado.
- Las facades se utilizan para validaciones y proyecciones síncronas autorizadas; los eventos implementan efectos derivados. Tracking solicita la ruta a través de una proyección ACL sin acceder directamente al repositorio de Shipments.
- UnitOfWork posee la transacción porque los controladores llaman directamente servicios Handle, como el ejemplo. No se registran command pipeline behaviors que nunca se ejecutarían con llamadas directas. Cortex se usa realmente para eventos.
- Se omite Microsoft.EntityFrameworkCore.Relational.Design 1.1.6 del ZIP, paquete histórico innecesario con EF 10. El paquete Design 10.0.14 sí está incluido. Scalar se conserva en la versión compatible validada del proyecto.
- No se copian Profiles/Publishing: no pertenecen al dominio AgroFlet. El perfil de cuenta sigue en Iam.

## Semántica exacta de los eventos y las diapositivas

Los agregados registran eventos; no los publican los controladores. SavingChangesAsync los despacha antes del commit. Los handlers actualizan historial, flota, ETA y notificaciones dentro de la misma transacción. Los eventos derivados se procesan hasta agotar la cola, con límite para detectar ciclos. Si un handler o el guardado falla, se revierte la transacción. Para obtener IDs autogenerados puede existir un guardado intermedio dentro de esa transacción.

La afirmación de las diapositivas de que el interceptor publica exclusivamente después de confirmar la base no coincide con SavingChangesAsync. Esta entrega no repite esa garantía: los efectos externos irreversibles no pertenecen a estos handlers. Un broker/outbox no está implementado ni es necesario para los efectos actuales exclusivamente en MySQL.

## Orientación a servicios

La API expone servicios REST con contratos y casos de uso separados; internamente es un monolito modular con bounded contexts y eventos en proceso, igual al enfoque general del ejemplo. No se presenta como una arquitectura de microservicios desplegados ni como mensajería distribuida.
