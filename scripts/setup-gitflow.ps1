<#
.SYNOPSIS
    Configura el flujo de trabajo GitFlow-Next, plantilla de confirmación y hooks locales.

.DESCRIPTION
    Este script automatiza la configuración del repositorio tras ser clonado:
    1. Verifica la presencia de Git y la extensión git-flow-next.
    2. Inicializa GitFlow con las convenciones de ramas de la organización.
    3. Configura fusiones explícitas sin avance rápido (no-ff).
    4. Estandariza los mensajes de fusión según Conventional Commits v2.0.0.
    5. Activa la plantilla .gitmessage y el hook de validación .githooks/commit-msg.
    6. Posiciona al desarrollador en la rama de integración 'develop'.

.EXAMPLE
    .\scripts\setup-gitflow.ps1
#>

[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

function Write-Step {
    param([string]$Message)
    Write-Host "`n[+] $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "    [OK] $Message" -ForegroundColor Green
}

function Write-WarningMessage {
    param([string]$Message)
    Write-Host "    [!] $Message" -ForegroundColor Yellow
}

function Write-ErrorMessage {
    param([string]$Message)
    Write-Host "`n[ERROR] $Message" -ForegroundColor Red
}

Write-Host "============================================================" -ForegroundColor Magenta
Write-Host "  XPressPDF - Configuración de GitFlow-Next y Convenciones  " -ForegroundColor Magenta
Write-Host "============================================================" -ForegroundColor Magenta

# 1. Validar entorno Git y posicionarse en la raíz del repositorio
Write-Step "Comprobando entorno de Git..."
try {
    $repoRoot = git rev-parse --show-toplevel 2>$null
    if (-not $repoRoot) {
        throw "No se pudo detectar la raíz del repositorio Git."
    }
    Set-Location $repoRoot
    Write-Success "Raíz del repositorio detectada: $repoRoot"
} catch {
    Write-ErrorMessage "El directorio actual no es un repositorio Git válido."
    exit 1
}

# 2. Validar que git-flow-next esté instalado
Write-Step "Verificando extensión git-flow-next..."
try {
    $gitFlowVer = git flow version 2>$null
    if (-not $gitFlowVer) {
        throw "git flow version no devolvió resultado."
    }
    Write-Success "git-flow-next detectado: $gitFlowVer"
} catch {
    Write-ErrorMessage "La extensión git-flow-next no está instalada o no se encuentra en el PATH."
    Write-Host "`nPara instalarla en Windows ejecute:" -ForegroundColor Yellow
    Write-Host "  winget install GitTower.GitFlowNext" -ForegroundColor White
    Write-Host "  o bien:" -ForegroundColor Yellow
    Write-Host "  scoop install git-flow-next`n" -ForegroundColor White
    Write-Host "En macOS / Linux (Homebrew):" -ForegroundColor Yellow
    Write-Host "  brew install gittower/tap/git-flow-next`n" -ForegroundColor White
    exit 1
}

# 3. Asegurar ramas base locales (main y develop)
Write-Step "Asegurando ramas base (main y develop)..."

# Verificar o crear rama main
$hasMain = git branch --list main
if (-not $hasMain) {
    $hasRemoteMain = git branch -r --list "origin/main"
    if ($hasRemoteMain) {
        git branch --track main origin/main | Out-Null
        Write-Success "Rama 'main' vinculada con origin/main."
    } else {
        # Si aún no existe remotamente, asegurarse de tener la rama actual
        $currentBranch = git branch --show-current
        if ($currentBranch -ne "main") {
            git branch -M main | Out-Null
        }
        Write-Success "Rama local 'main' establecida."
    }
} else {
    Write-Success "Rama local 'main' disponible."
}

# Verificar o crear rama develop
$hasDevelop = git branch --list develop
if (-not $hasDevelop) {
    $hasRemoteDevelop = git branch -r --list "origin/develop"
    if ($hasRemoteDevelop) {
        git branch --track develop origin/develop | Out-Null
        Write-Success "Rama 'develop' vinculada con origin/develop."
    } else {
        git branch develop main | Out-Null
        Write-Success "Rama local 'develop' creada a partir de 'main'."
    }
} else {
    Write-Success "Rama local 'develop' disponible."
}

# 4. Inicializar GitFlow-Next
Write-Step "Inicializando configuración de GitFlow-Next..."
git flow init -f --preset=classic `
    --main main `
    --develop develop `
    --feature "feature/" `
    --bugfix "bugfix/" `
    --release "release/" `
    --hotfix "hotfix/" `
    --support "support/" `
    --tag "v" | Out-Null

Write-Success "Prefijos de ramas y ramas base inicializadas con éxito."

# 5. Configurar fusiones sin avance rápido (no-ff)
Write-Step "Configurando fusiones sin avance rápido (--no-ff)..."
$branchTypes = @("feature", "bugfix", "release", "hotfix", "support")
foreach ($type in $branchTypes) {
    git config "gitflow.$type.finish.no-ff" true
}
Write-Success "Fusiones no-ff forzadas para feature, bugfix, release, hotfix y support."

# 6. Configurar mensajes de fusión según Conventional Commits v2.0.0
Write-Step "Configurando mensajes estandarizados de fusión (mergemessage / updatemessage)..."
$mergeMessage = "chore(merge): integrar %b en %p"

foreach ($type in $branchTypes) {
    git config "gitflow.$type.finish.mergemessage" $mergeMessage
}

git config "gitflow.release.finish.updatemessage" $mergeMessage
git config "gitflow.hotfix.finish.updatemessage" $mergeMessage

Write-Success "Mensajes de fusión configurados con formato: '$mergeMessage'."

# 7. Configurar plantilla de mensajes de commit
Write-Step "Activando plantilla de confirmación (.gitmessage)..."
if (Test-Path ".gitmessage") {
    git config commit.template .gitmessage
    Write-Success "Plantilla commit.template asociada a .gitmessage."
} else {
    Write-WarningMessage "No se encontró el archivo .gitmessage en la raíz."
}

# 8. Configurar ruta de hooks locales
Write-Step "Activando hooks de validación locales (.githooks)..."
if (Test-Path ".githooks/commit-msg") {
    git config core.hooksPath .githooks
    Write-Success "core.hooksPath establecido en .githooks."
} else {
    Write-WarningMessage "No se encontró el hook .githooks/commit-msg."
}

# 9. Cambiar a la rama de integración develop
Write-Step "Cambiando a la rama de integración 'develop'..."
git switch develop | Out-Null
Write-Success "Rama activa actual: $(git branch --show-current)"

# 10. Resumen y verificación final
Write-Step "Verificando parámetros aplicados en el repositorio..."
$configuredKeys = git config --get-regexp '^gitflow\..*finish'
$templateKey = git config commit.template
$hooksKey = git config core.hooksPath

Write-Host "`n------------------- Resumen de Configuración -------------------" -ForegroundColor DarkGray
Write-Host "Ramas base:          main (producción), develop (integración)" -ForegroundColor White
Write-Host "Prefijos:            feature/, bugfix/, release/, hotfix/, support/" -ForegroundColor White
Write-Host "Prefijo de versión:  v (SemVer)" -ForegroundColor White
Write-Host "commit.template:     $templateKey" -ForegroundColor White
Write-Host "core.hooksPath:      $hooksKey" -ForegroundColor White
Write-Host "Claves de cierre:    $($configuredKeys.Count) claves 'finish' configuradas" -ForegroundColor White
Write-Host "----------------------------------------------------------------" -ForegroundColor DarkGray

Write-Host "`n[✓] Repositorio configurado exitosamente según las políticas internas.`n" -ForegroundColor Green
