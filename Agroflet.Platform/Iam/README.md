# IAM: autenticación y usuarios

## Flujo de una solicitud

`AuthenticationController` transforma Resources en Commands y delega en
`AuthenticationCommandService`. El servicio usa `ICredentialService`, repositorios
 y `IUnitOfWork`; los controladores convierten `Result<T>` en respuestas HTTP.
`UsersController` concentra las consultas y el perfil del usuario autenticado.

## Endpoints principales

- `POST /api/v1/auth/register`: registra una cuenta; correo duplicado produce 409.
- `POST /api/v1/auth/login`: normaliza el correo; credenciales inválidas producen 401.
- `POST /api/v1/auth/logout`: revoca el JWT mediante su identificador `jti`.
- `GET /api/v1/users/profile`: consulta el perfil de la identidad autenticada.
- `PUT /api/v1/users/profile`: actualiza el perfil propio.
- `PUT /api/v1/users/password`: requiere la contraseña actual.
- `POST /api/v1/auth/password-reset-requests`: responde 202 con un mensaje genérico.
- `POST /api/v1/auth/password-resets`: consume el token de recuperación.

Los alias bajo `/api/v1/authentication` están definidos en el controlador.
Los esquemas completos se consultan en `/scalar/v1` y `/openapi/v1.json`.

## Reglas y seguridad

El servicio limita la edición de perfil al propio `actor.Id`; un identificador
ajeno responde 404. La recuperación almacena SHA-256 del token, no el token en
texto plano, y consume la solicitud dentro de una transacción. La entrega SMTP
es una dependencia externa: registrar una cuenta no acredita que el correo funcione.

La composición JWT y el limitador `auth` se configuran en `Program.cs`.
En producción se requiere `Jwt__Key` de al menos 32 bytes y se mantiene
`PasswordReset__ReturnTokenInDevelopment=false`. Nunca guardar secretos en Git.

## Verificación manual propuesta

1. Registrar un usuario de prueba y comprobar el rechazo del mismo correo.
2. Iniciar sesión y consultar el perfil con Bearer JWT.
3. Intentar editar un perfil ajeno y verificar 404.
4. Cerrar sesión y verificar que el JWT revocado ya no autoriza solicitudes.

Estos pasos son una guía de comprobación, no una afirmación de ejecución remota.
