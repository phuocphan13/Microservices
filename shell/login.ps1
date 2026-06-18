# Prompt for SSH username interactively
$Username = Read-Host 'Enter SSH username'

function Get-SshProcesses([string]$user) {
    $host = 'nextcloudsg.ddns.net'
    $filter = "Name LIKE 'ssh%'"
    $procs = Get-CimInstance Win32_Process | Where-Object { $_.Name -like 'ssh*' -and $_.CommandLine -and ($_.CommandLine -match $host) }
    if ($user) {
        $procs = $procs | Where-Object { $_.CommandLine -match "$user@$host" }
    }
    return $procs
}

function Stop-Tunnel([string]$user) {
    $procs = Get-SshProcesses -user $user
    if (-not $procs) {
        Write-Host "No matching ssh tunnel processes found. Nothing to stop."
        return
    }
    foreach ($p in $procs) {
        try {
            Stop-Process -Id $p.ProcessId -Force -ErrorAction Stop
            Write-Host "Stopped ssh tunnel (pid $($p.ProcessId))."
        } catch {
            Write-Warning "Could not stop process $($p.ProcessId): $_"
        }
    }
}

function Start-Tunnel([string]$user) {
    if (-not $user) {
        $user = Read-Host 'Enter SSH username'
    }

    $host = 'nextcloudsg.ddns.net'
    $port = 18890

    Write-Host "`nConnecting to $host as $user ...`n"

    Write-Host '================= PORT MAPPING ================'
    Write-Host 'IdentityServer  -> http://localhost:8081'
    Write-Host 'PgAdmin         -> http://localhost:8082'
    Write-Host 'Portainer       -> http://localhost:9000'
    Write-Host ''
    Write-Host 'Catalog API     -> http://localhost:5001'
    Write-Host 'Basket API      -> http://localhost:5002'
    Write-Host 'Discount API    -> http://localhost:5003'
    Write-Host 'Discount GRPC   -> http://localhost:5004'
    Write-Host 'Ordering API    -> http://localhost:5005'
    Write-Host ''
    Write-Host 'MongoDB         -> localhost:27017'
    Write-Host 'Redis (basket)  -> localhost:6379'
    Write-Host 'Redis (catalog) -> localhost:6380'
    Write-Host 'PostgreSQL      -> localhost:5432'
    Write-Host 'MSSQL           -> localhost:1433'
    Write-Host 'RabbitMQ AMQP   -> localhost:5672'
    Write-Host 'RabbitMQ UI     -> http://localhost:15672'
    Write-Host '=============================================='
    Write-Host ''

    Write-Host 'Starting persistent background tunnels (toggle mode). You may be prompted for SSH password.'

    $tunnelArgs = @(
        '-N',
        '-p', $port,
        '-o', 'PreferredAuthentications=password,keyboard-interactive',
        '-o', 'PubkeyAuthentication=no',
        "$user@$host",
        '-L', '8081:localhost:80',
        '-L', '8082:localhost:80',
        '-L', '9000:localhost:9000',
        '-L', '5001:localhost:5001',
        '-L', '5002:localhost:5002',
        '-L', '5003:localhost:5003',
        '-L', '5004:localhost:5004',
        '-L', '5005:localhost:5005',
        '-L', '27017:localhost:27017',
        '-L', '6379:localhost:6379',
        '-L', '6380:localhost:6379',
        '-L', '5432:localhost:5432',
        '-L', '1433:localhost:1433',
        '-L', '5672:localhost:5672',
        '-L', '15672:localhost:15672'
    )

    $argString = $tunnelArgs -join ' '
    Write-Host "Starting SSH tunnel for $user@${host} (background). You may be prompted for password."
    $proc = Start-Process -FilePath ssh -ArgumentList $argString -WindowStyle Hidden -PassThru
    if ($proc -ne $null) {
        Write-Host "Tunnel started (pid: $($proc.Id)). Local ports are available on localhost."
    } else {
        Write-Error 'Failed to start ssh process.'
    }
}

# Toggle: if matching ssh process exists, stop; otherwise start
$existing = Get-SshProcesses -user $Username
if ($existing) {
    Write-Host "Tunnel appears to be running (found $($existing.Count) matching process(es)). Stopping..."
    Stop-Tunnel -user $Username
    Write-Host 'Done. Local ports are no longer forwarded.'
    exit 0
} else {
    Start-Tunnel -user $Username
    Write-Host 'Done. Local ports are forwarded to remote services.'
    exit 0
}
