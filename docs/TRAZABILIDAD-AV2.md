# Trazabilidad del backend AV2

Fuente revisada el 11 de noviembre de 2026: [Capítulo III](https://github.com/AGROFLET/Agroflet-report/blob/main/03-Chapter-3-Solution-UI-UX-Design.md), blob e36be99b29a32292f110ded1a362dc6bd72b8daa. No se identificó una selección explícita etiquetada AV2: se toma como alcance el soporte de las 27 Technical Stories solicitadas, sin atribuir aceptación oficial del docente.

| Historia | Contrato | User Stories |
|---|---|---|
| **TS01** | POST /api/v1/auth/register | US01 |
| **TS02** | POST /api/v1/auth/login | US02 |
| **TS03** | GET /api/v1/shipments | US10 |
| **TS04** | POST /api/v1/shipments | US09 |
| **TS05** | GET /api/v1/shipments/{id} | US11 |
| **TS06** | PATCH /api/v1/shipments/{id}/status | US12, US31 |
| **TS07** | POST /api/v1/shipments/{id}/incidents | US13 |
| **TS08** | GET /api/v1/shipments/{id}/incidents | US14 |
| **TS09** | GET /api/v1/vehicles | US05, US08 |
| **TS10** | POST /api/v1/vehicles | US05 |
| **TS11** | GET /api/v1/drivers | US07, US08 |
| **TS12** | POST /api/v1/drivers | US07 |
| **TS13** | GET /api/v1/shipments con filtros | US15, US16 |
| **TS14** | GET /api/v1/shipments/active/locations | US17 |
| **TS15** | GET /api/v1/notifications | US19, US20 |
| **TS16** | PATCH /api/v1/notifications/{id}/read | US19, US20 |
| **TS17** | GET /api/v1/users/profile | US21, US23 |
| **TS18** | PUT /api/v1/users/profile | US21, US23 |
| **TS19** | PATCH /api/v1/vehicles/{id}/status | US06 |
| **TS20** | GET /api/v1/shipments/{id}/status-history | US11 |
| **TS21** | POST /api/v1/auth/logout | US03 |
| **TS22** | POST /api/v1/auth/password-reset-requests | US04 |
| **TS23** | POST /api/v1/auth/password-resets | US04 |
| **TS24** | PUT /api/v1/users/password | US22 |
| **TS25** | PUT /api/v1/vehicles/{id} | US06 |
| **TS26** | POST /api/v1/shipments/{id}/positions | US32 |
| **TS27** | POST /api/v1/contact-messages | US28 |

Además se implementa GET /api/v1/shipments/{id}/planned-route para US18 mediante Mapbox Directions v5. Los alias /authentication y las colecciones históricas se conservan para compatibilidad de contratos, sin incluir ni modificar el frontend.

La cobertura ejecutada se detalla en VALIDACION.md. Tener un endpoint no acredita por sí solo aceptación E2E de la historia ni despliegue. La plantilla no justifica copiar Profiles o Publishing al dominio agrícola.

## Diferencias documentadas

- Las colecciones de flota devuelven al comprador únicamente proyecciones de recursos asociados, para soportar US11; no se entrega DNI del conductor. Ajustar criterios que exijan 403 para toda consulta de flota si se mantiene esta UX.
- La cancelación conserva historial: PATCH con motivo, no DELETE físico. El DELETE de compensación del prototipo se sustituye por rollback transaccional.
- PUT de vehículo conserva la semántica de actualización de campos editables de TS25; PATCH está disponible para cambios parciales y estado. Las operaciones reservadas bloquean cambios de flota incompatibles.
- Los idiomas en_US/es_419 se normalizan a en/es.
- US28 se respalda con contacto persistente y rate limiting; la landing no se modifica en este trabajo. El capítulo V contiene menciones contradictorias a pendiente y finalizada; usar evidencia de una petición real antes de declarar su aceptación completa.

