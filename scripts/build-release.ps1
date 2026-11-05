<#
.SYNOPSIS
    Compila y empaqueta las versiones oficiales de lanzamiento (Releases) de XPress PDF.

.DESCRIPTION
    Genera distribuciones multiplataforma para las arquitecturas soportadas:
    - Windows: win-x64, win-arm64 (.zip)
    - Linux: linux-x64, linux-arm64 (.tar.gz)
    - macOS: osx-x64, osx-arm64 (.tar.gz)

    Para cada arquitectura compila dos modalidades:
    1. Standalone (Autónoma): Incluye el runtime de .NET 10 (Single-File, Self-Contained).
    2. Portable (Ligera): Requiere tener instalado el .NET 10 Runtime en el sistema.

    Calcula las firmas criptográficas SHA-256 y genera el archivo SHA256SUMS.txt.

.PARAMETER Version
    Versión SemVer a publicar (por ejemplo, "1.0.0"). Por defecto toma "1.0.0".

.PARAMETER Runtime
    Identificador de runtime (RID) a compilar. Opciones: "all", "win-x64", "win-arm64", "linux-x64", "linux-arm64", "osx-x64", "osx-arm64". Por defecto es "all".

.PARAMETER OutputDir
    Carpeta destino donde se guardarán los instaladores y checksums. Por defecto es "./dist".

.PARAMETER Clean
    Si se especifica, limpia la carpeta de salida antes de compilar.

.EXAMPLE
    .\scripts\build-release.ps1 -Version "1.0.0" -Clean
    .\scripts\build-release.ps1 -Version "1.0.0" -Runtime "win-x64" -Clean
#>

[CmdletBinding()]
param(
    [string]$Version = "1.0.0",
    [string]$Runtime = "all",
    [string]$OutputDir = "dist",
    [switch]$Clean
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

Write-Host "`n========================================================" -ForegroundColor Cyan
Write-Host " [XPress PDF] Preparador de Releases (v$Version)" -ForegroundColor Cyan
Write-Host "========================================================`n" -ForegroundColor Cyan

# 1. Validar presencia del SDK de .NET
try {
    $dotnetVersion = dotnet --version
    Write-Host "[OK] .NET SDK detectado: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Error "No se encontro el comando 'dotnet'. Asegurate de tener instalado el .NET 10 SDK."
    exit 1
}

$allRuntimes = @("win-x64", "win-arm64", "linux-x64", "linux-arm64", "osx-x64", "osx-arm64")

$targetRuntimes = if ($Runtime -eq "all") {
    $allRuntimes
} else {
    $selected = $Runtime -split "[,;\s]+" | Where-Object { $_ -ne "" }
    foreach ($r in $selected) {
        if ($allRuntimes -notcontains $r) {
            Write-Error "Runtime no soportado: '$r'. Opciones válidas: $($allRuntimes -join ', ')"
            exit 1
        }
    }
    $selected
}

$rootPath = Resolve-Path "$PSScriptRoot/.."
$projectPath = "$rootPath/AppUI/AppUI.csproj"
$resolvedOutputDir = [System.IO.Path]::GetFullPath((Join-Path $rootPath $OutputDir))

if ($Clean -and (Test-Path $resolvedOutputDir)) {
    Write-Host "[INFO] Limpiando directorio de salida previo: $resolvedOutputDir" -ForegroundColor Yellow
    Remove-Item -Recurse -Force $resolvedOutputDir
}

if (-not (Test-Path $resolvedOutputDir)) {
    New-Item -ItemType Directory -Force -Path $resolvedOutputDir | Out-Null
}

$generatedPackages = [System.Collections.Generic.List[string]]::new()

foreach ($rid in $targetRuntimes) {
    Write-Host "`n--------------------------------------------------------" -ForegroundColor DarkCyan
    Write-Host " Procesando Arquitectura: $rid" -ForegroundColor Cyan
    Write-Host "--------------------------------------------------------" -ForegroundColor DarkCyan

    $targetIsWindows = $rid.StartsWith("win-")
    $targetIsLinux = $rid.StartsWith("linux-")
    $targetIsOsx = $rid.StartsWith("osx-")

    $exeExt = if ($targetIsWindows) { ".exe" } else { "" }
    $originalExe = "AppUI$exeExt"
    $targetExe = "XPressPDF$exeExt"

    # Preparar carpetas temporales
    $stageStandalone = Join-Path $resolvedOutputDir "stage_${rid}_standalone"
    $stagePortable = Join-Path $resolvedOutputDir "stage_${rid}_portable"

    if (Test-Path $stageStandalone) { Remove-Item -Recurse -Force $stageStandalone }
    if (Test-Path $stagePortable) { Remove-Item -Recurse -Force $stagePortable }

    New-Item -ItemType Directory -Force -Path $stageStandalone | Out-Null
    New-Item -ItemType Directory -Force -Path $stagePortable | Out-Null

    # Solo habilitar ReadyToRun si el OS actual coincide con la familia destino para evitar fallos de cross-compilation
    $enableR2R = ($targetIsWindows -and [System.OperatingSystem]::IsWindows()) -or
                 ($targetIsLinux -and [System.OperatingSystem]::IsLinux()) -or
                 ($targetIsOsx -and [System.OperatingSystem]::IsMacOS())

    # A) Standalone (Self-Contained)
    Write-Host "[1/2] Compilando Standalone ($rid)..." -ForegroundColor DarkGray
    $publishArgsStandalone = @(
        "publish", $projectPath,
        "-c", "Release",
        "-r", $rid,
        "--self-contained", "true",
        "-p:PublishSingleFile=true",
        "-p:IncludeNativeLibrariesForSelfExtract=true",
        "-p:Version=$Version",
        "-p:DebugType=embedded",
        "-o", $stageStandalone
    )
    if ($enableR2R) {
        $publishArgsStandalone += "-p:PublishReadyToRun=true"
    }

    & dotnet $publishArgsStandalone
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Error al compilar la distribucion Standalone para $rid."
        exit $LASTEXITCODE
    }

    # B) Portable (Framework-Dependent)
    Write-Host "[2/2] Compilando Portable ($rid)..." -ForegroundColor DarkGray
    $publishArgsPortable = @(
        "publish", $projectPath,
        "-c", "Release",
        "-r", $rid,
        "--self-contained", "false",
        "-p:PublishSingleFile=true",
        "-p:Version=$Version",
        "-p:DebugType=embedded",
        "-o", $stagePortable
    )

    & dotnet $publishArgsPortable
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Error al compilar la distribucion Portable para $rid."
        exit $LASTEXITCODE
    }

    # Renombrar ejecutable a XPressPDF
    foreach ($stage in @($stageStandalone, $stagePortable)) {
        $srcPath = Join-Path $stage $originalExe
        $dstPath = Join-Path $stage $targetExe
        if (Test-Path $srcPath) {
            Move-Item -Path $srcPath -Destination $dstPath -Force
        }
    }

    # Empaquetado
    if ($targetIsWindows) {
        # Empaquetar como .zip
        $standalonePkgName = "XPressPDF-v$Version-$rid-standalone.zip"
        $portablePkgName = "XPressPDF-v$Version-$rid-portable.zip"

        $standalonePkgPath = Join-Path $resolvedOutputDir $standalonePkgName
        $portablePkgPath = Join-Path $resolvedOutputDir $portablePkgName

        Compress-Archive -Path "$stageStandalone/*" -DestinationPath $standalonePkgPath -Force
        Compress-Archive -Path "$stagePortable/*" -DestinationPath $portablePkgPath -Force

        $generatedPackages.Add($standalonePkgPath)
        $generatedPackages.Add($portablePkgPath)
    } else {
        # Empaquetar como .tar.gz para preservar permisos y atributos en Unix
        $standalonePkgName = "XPressPDF-v$Version-$rid-standalone.tar.gz"
        $portablePkgName = "XPressPDF-v$Version-$rid-portable.tar.gz"

        $standalonePkgPath = Join-Path $resolvedOutputDir $standalonePkgName
        $portablePkgPath = Join-Path $resolvedOutputDir $portablePkgName

        # Verificar si tar está disponible en el sistema
        $tarAvailable = Get-Command tar -ErrorAction SilentlyContinue

        if ($tarAvailable) {
            & tar -czf $standalonePkgPath -C $stageStandalone .
            & tar -czf $portablePkgPath -C $stagePortable .
        } else {
            # Fallback en caso de entornos Windows antiguos sin comando tar
            $standalonePkgName = "XPressPDF-v$Version-$rid-standalone.zip"
            $portablePkgName = "XPressPDF-v$Version-$rid-portable.zip"
            $standalonePkgPath = Join-Path $resolvedOutputDir $standalonePkgName
            $portablePkgPath = Join-Path $resolvedOutputDir $portablePkgName
            Compress-Archive -Path "$stageStandalone/*" -DestinationPath $standalonePkgPath -Force
            Compress-Archive -Path "$stagePortable/*" -DestinationPath $portablePkgPath -Force
        }

        $generatedPackages.Add($standalonePkgPath)
        $generatedPackages.Add($portablePkgPath)
    }

    # Limpiar directorios de staging
    Remove-Item -Recurse -Force $stageStandalone
    Remove-Item -Recurse -Force $stagePortable

    Write-Host "[OK] Empaquetado completado para $rid" -ForegroundColor Green
}

# 2. Generar Checksums SHA-256
Write-Host "`n[+] Calculando firmas criptográficas (SHA-256)..." -ForegroundColor Cyan

$hashes = @()
foreach ($pkg in $generatedPackages) {
    if (Test-Path $pkg) {
        $hash = Get-FileHash -Path $pkg -Algorithm SHA256
        $hashes += $hash
    }
}

$checksumFile = Join-Path $resolvedOutputDir "SHA256SUMS.txt"
$hashes | ForEach-Object { "$($_.Hash)  $($_.Path | Split-Path -Leaf)" } | Out-File -FilePath $checksumFile -Encoding utf8

Write-Host "`n========================================================" -ForegroundColor Green
Write-Host " [ÉXITO] Release preparado en $resolvedOutputDir" -ForegroundColor Green
Write-Host "========================================================`n" -ForegroundColor Green

Write-Host "Tabla de verificación SHA-256:`n" -ForegroundColor Cyan
Write-Host "| Archivo | Tamaño | SHA-256 |"
Write-Host "| :--- | :---: | :--- |"
foreach ($item in $hashes) {
    $fileInfo = Get-Item $item.Path
    $sizeMB = ($fileInfo.Length / 1MB).ToString("N2") + " MB"
    Write-Host "| $($fileInfo.Name) | $sizeMB | $($item.Hash) |"
}
Write-Host ""
