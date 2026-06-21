$Username = Read-Host 'Enter SSH username'
$SshHost = 'nextcloudsg.ddns.net'
$Port = 18890

Write-Host "`nConnecting to $SshHost as $Username..."
Write-Host ''
Write-Host '================= PORT MAPPING ================'
Write-Host 'ArgoCD          -> https://localhost:8080'
Write-Host 'Catalog API     -> http://localhost:5001'
Write-Host 'Basket API      -> http://localhost:5002'
Write-Host 'Discount API    -> http://localhost:5003'
Write-Host 'Discount GRPC   -> http://localhost:5004'
Write-Host 'Ordering API    -> http://localhost:5005'
Write-Host 'PgAdmin         -> http://localhost:8082'
Write-Host 'Portainer       -> http://localhost:9000'
Write-Host 'RabbitMQ UI     -> http://localhost:15672'
Write-Host '==============================================='
Write-Host ''
Write-Host 'Enter your password and MFA when prompted...'
Write-Host 'After successful login, the window will appear to hang - THIS IS NORMAL.'
Write-Host 'The tunnel is running. Press Ctrl+C to disconnect.'
Write-Host ''

ssh -N -p $Port "$Username@$SshHost" `
    -L 127.0.0.1:8080:localhost:30102 `
    -L 127.0.0.1:5001:localhost:30001 `
    -L 127.0.0.1:5002:localhost:30002 `
    -L 127.0.0.1:5003:localhost:30003 `
    -L 127.0.0.1:5004:localhost:30004 `
    -L 127.0.0.1:5005:localhost:30005 `
    -L 127.0.0.1:8082:localhost:30800 `
    -L 127.0.0.1:9000:localhost:30900 `
    -L 127.0.0.1:15672:localhost:31672
