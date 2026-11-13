param(
    [string]$Servidor = 'localhost',
    [ValidateRange(1,65535)][int]$Puerto = 3306,
    [string]$BaseDeDatos = 'agroflet_learning_center',
    [string]$Usuario = 'root',
    [switch]$Actualizar
)
$ErrorActionPreference = 'Stop'
$destination = Join-Path $PSScriptRoot 'Agroflet.Platform/appsettings.Local.json'
$existing = $null
if ((Test-Path -LiteralPath $destination) -and -not $Actualizar) {
    throw 'Ya existe appsettings.Local.json. Edítalo en Rider; no se sobrescribirá tu configuración.'
}
if ((Test-Path -LiteralPath $destination) -and $Actualizar) {
    $existing = Get-Content -LiteralPath $destination -Raw | ConvertFrom-Json
    $previous = [System.Data.Common.DbConnectionStringBuilder]::new()
    $previous.set_ConnectionString($existing.ConnectionStrings.DefaultConnection)
    if (-not $PSBoundParameters.ContainsKey('Servidor')) { $Servidor = [string]$previous.get_Item('server') }
    if (-not $PSBoundParameters.ContainsKey('Puerto')) { $Puerto = [int]$previous.get_Item('port') }
    if (-not $PSBoundParameters.ContainsKey('BaseDeDatos')) { $BaseDeDatos = [string]$previous.get_Item('database') }
    if (-not $PSBoundParameters.ContainsKey('Usuario')) { $Usuario = [string]$previous.get_Item('user') }
}
$password = Read-Host 'Contraseña de tu usuario MySQL (no se mostrará)' -AsSecureString
$credential = [System.Net.NetworkCredential]::new('', $password)
$connection = [System.Data.Common.DbConnectionStringBuilder]::new()
$connection['server'] = $Servidor
$connection['port'] = $Puerto
$connection['database'] = $BaseDeDatos
$connection['user'] = $Usuario
$connection['password'] = $credential.Password
$keyBytes = New-Object byte[] 48
$generator = [System.Security.Cryptography.RandomNumberGenerator]::Create()
try { $generator.GetBytes($keyBytes) } finally { $generator.Dispose() }
$config = @{
    ConnectionStrings = @{ DefaultConnection = $connection.ConnectionString }
    Jwt = @{ Key = [Convert]::ToBase64String($keyBytes) }
    Mapbox = @{ AccessToken = '' }
}
if ($null -ne $existing) {
    # Preserve JWT, Mapbox, SMTP and any other local settings.
    $existing.ConnectionStrings.DefaultConnection = $connection.get_ConnectionString()
    $config = $existing
}
$json = $config | ConvertTo-Json -Depth 10
[IO.File]::WriteAllText($destination, $json, [Text.UTF8Encoding]::new($false))
Write-Host 'Configuración local creada. Abre agroflet_backend1.sln en Rider y ejecuta el perfil http.'
Write-Host 'La contraseña queda en appsettings.Local.json, excluido de Git. No compartas ese archivo.'
