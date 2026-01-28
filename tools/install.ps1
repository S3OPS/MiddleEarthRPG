param(
    [switch]$AutoRun
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Resolve-Path (Join-Path $scriptRoot "..")
$streamingConfig = Join-Path $projectRoot "Assets\StreamingAssets\rpg_config.json"

$localAppData = $env:LOCALAPPDATA
if ([string]::IsNullOrWhiteSpace($localAppData)) {
  $localAppData = if ($env:XDG_DATA_HOME) {
    $env:XDG_DATA_HOME
  } elseif ($env:HOME) {
    Join-Path $env:HOME ".local/share"
  } else {
    $projectRoot
  }
}

$userRoot = Join-Path $localAppData "MiddleEarthRPG"
$configTarget = Join-Path $userRoot "rpg_config.json"
$runScript = Join-Path $userRoot "run.ps1"

New-Item -ItemType Directory -Force -Path $userRoot | Out-Null
Copy-Item $streamingConfig $configTarget -Force

@"
Write-Host "Launching Middle-earth Adventure RPG..."
`$unityHub = "${env:ProgramFiles}\Unity Hub\Unity Hub.exe"
if (Test-Path `$unityHub) {
  Start-Process -FilePath `$unityHub -ArgumentList "--", "-projectPath", '$projectRoot'
} else {
  Write-Host "Unity Hub not found. Open the project manually in Unity Hub: $projectRoot"
}
"@ | Set-Content -Path $runScript -Encoding UTF8

Write-Host "Config initialized at: $configTarget"
Write-Host "Run script created at: $runScript"

if ($AutoRun) {
  & $runScript
}
