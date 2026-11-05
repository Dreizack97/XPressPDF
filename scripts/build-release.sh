#!/usr/bin/env bash
#
# Compila y empaqueta las versiones oficiales de lanzamiento (Releases) de XPress PDF.
#

set -euo pipefail

CYAN='\033[0;36m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

VERSION="1.0.0"
RUNTIME="all"
OUTPUT_DIR="dist"
CLEAN=false

while [[ $# -gt 0 ]]; do
    case "$1" in
        -v|--version)
            VERSION="$2"
            shift 2
            ;;
        -r|--runtime)
            RUNTIME="$2"
            shift 2
            ;;
        -o|--output-dir)
            OUTPUT_DIR="$2"
            shift 2
            ;;
        -c|--clean)
            CLEAN=true
            shift
            ;;
        *)
            echo -e "${RED}Opción no reconocida: $1${NC}" >&2
            exit 1
            ;;
    esac
done

printf "\n${CYAN}========================================================${NC}\n"
printf "${CYAN} [XPress PDF] Preparador de Releases (v%s)${NC}\n" "$VERSION"
printf "${CYAN}========================================================${NC}\n\n"

# 1. Validar .NET SDK
if ! dotnet_ver="$(dotnet --version 2>/dev/null)"; then
    echo -e "${RED}No se encontró el comando 'dotnet'. Asegúrate de tener instalado el .NET 10 SDK.${NC}" >&2
    exit 1
fi
printf "${GREEN}[OK] .NET SDK detectado: %s${NC}\n" "$dotnet_ver"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_PATH="$ROOT_DIR/AppUI/AppUI.csproj"
RESOLVED_OUT_DIR="$ROOT_DIR/$OUTPUT_DIR"

ALL_RUNTIMES=("win-x64" "win-arm64" "linux-x64" "linux-arm64" "osx-x64" "osx-arm64")

TARGET_RUNTIMES=()
if [[ "$RUNTIME" == "all" ]]; then
    TARGET_RUNTIMES=("${ALL_RUNTIMES[@]}")
else
    IFS=', ' read -r -a SPLIT_RUNTIMES <<< "$RUNTIME"
    for r in "${SPLIT_RUNTIMES[@]}"; do
        valid=false
        for a in "${ALL_RUNTIMES[@]}"; do
            if [[ "$r" == "$a" ]]; then
                valid=true
                break
            fi
        done
        if [[ "$valid" == false ]]; then
            echo -e "${RED}Runtime no soportado: '$r'. Opciones válidas: ${ALL_RUNTIMES[*]}${NC}" >&2
            exit 1
        fi
        TARGET_RUNTIMES+=("$r")
    done
fi

if [[ "$CLEAN" == true && -d "$RESOLVED_OUT_DIR" ]]; then
    printf "${YELLOW}[INFO] Limpiando directorio de salida previo: %s${NC}\n" "$RESOLVED_OUT_DIR"
    rm -rf "$RESOLVED_OUT_DIR"
fi

mkdir -p "$RESOLVED_OUT_DIR"

GENERATED_PACKAGES=()

# Detectar OS actual del host
CURRENT_OS="$(uname -s 2>/dev/null || echo "Unknown")"

for rid in "${TARGET_RUNTIMES[@]}"; do
    printf "\n${CYAN}--------------------------------------------------------${NC}\n"
    printf "${CYAN} Procesando Arquitectura: %s${NC}\n" "$rid"
    printf "${CYAN}--------------------------------------------------------${NC}\n"

    is_windows=false
    is_linux=false
    is_osx=false

    if [[ "$rid" == win-* ]]; then is_windows=true; fi
    if [[ "$rid" == linux-* ]]; then is_linux=true; fi
    if [[ "$rid" == osx-* ]]; then is_osx=true; fi

    exe_ext=""
    if [[ "$is_windows" == true ]]; then exe_ext=".exe"; fi

    stage_standalone="$RESOLVED_OUT_DIR/stage_${rid}_standalone"
    stage_portable="$RESOLVED_OUT_DIR/stage_${rid}_portable"

    rm -rf "$stage_standalone" "$stage_portable"
    mkdir -p "$stage_standalone" "$stage_portable"

    # ReadyToRun solo si el OS host coincide
    enable_r2r=false
    if [[ "$is_linux" == true && "$CURRENT_OS" == "Linux" ]]; then enable_r2r=true; fi
    if [[ "$is_osx" == true && "$CURRENT_OS" == "Darwin" ]]; then enable_r2r=true; fi

    # A) Standalone
    printf "[1/2] Compilando Standalone (%s)...\n" "$rid"
    standalone_args=(
        "publish" "$PROJECT_PATH"
        "-c" "Release"
        "-r" "$rid"
        "--self-contained" "true"
        "-p:PublishSingleFile=true"
        "-p:IncludeNativeLibrariesForSelfExtract=true"
        "-p:Version=$VERSION"
        "-p:DebugType=embedded"
        "-o" "$stage_standalone"
    )
    if [[ "$enable_r2r" == true ]]; then
        standalone_args+=("-p:PublishReadyToRun=true")
    fi

    dotnet "${standalone_args[@]}"

    # B) Portable
    printf "[2/2] Compilando Portable (%s)...\n" "$rid"
    dotnet publish "$PROJECT_PATH" \
        -c Release \
        -r "$rid" \
        "--self-contained" false \
        "-p:PublishSingleFile=true" \
        "-p:Version=$VERSION" \
        "-p:DebugType=embedded" \
        "-o" "$stage_portable"

    # Renombrar AppUI -> XPressPDF y asegurar permisos de ejecución
    for stage in "$stage_standalone" "$stage_portable"; do
        if [[ -f "$stage/AppUI$exe_ext" ]]; then
            mv "$stage/AppUI$exe_ext" "$stage/XPressPDF$exe_ext"
        fi
        if [[ "$is_windows" == false && -f "$stage/XPressPDF" ]]; then
            chmod +x "$stage/XPressPDF"
        fi
    done

    # Empaquetado
    if [[ "$is_windows" == true ]]; then
        standalone_pkg="XPressPDF-v$VERSION-$rid-standalone.zip"
        portable_pkg="XPressPDF-v$VERSION-$rid-portable.zip"
        standalone_path="$RESOLVED_OUT_DIR/$standalone_pkg"
        portable_path="$RESOLVED_OUT_DIR/$portable_pkg"

        (cd "$stage_standalone" && zip -r -q "$standalone_path" .)
        (cd "$stage_portable" && zip -r -q "$portable_path" .)

        GENERATED_PACKAGES+=("$standalone_path" "$portable_path")
    else
        standalone_pkg="XPressPDF-v$VERSION-$rid-standalone.tar.gz"
        portable_pkg="XPressPDF-v$VERSION-$rid-portable.tar.gz"
        standalone_path="$RESOLVED_OUT_DIR/$standalone_pkg"
        portable_path="$RESOLVED_OUT_DIR/$portable_pkg"

        tar -czf "$standalone_path" -C "$stage_standalone" .
        tar -czf "$portable_path" -C "$stage_portable" .

        GENERATED_PACKAGES+=("$standalone_path" "$portable_path")
    fi

    rm -rf "$stage_standalone" "$stage_portable"
    printf "${GREEN}[OK] Empaquetado completado para %s${NC}\n" "$rid"
done

# Checksums SHA-256
printf "\n${CYAN}[+] Calculando firmas criptográficas (SHA-256)...${NC}\n"
CHECKSUM_FILE="$RESOLVED_OUT_DIR/SHA256SUMS.txt"
rm -f "$CHECKSUM_FILE"

for pkg in "${GENERATED_PACKAGES[@]}"; do
    if [[ -f "$pkg" ]]; then
        pkg_name="$(basename "$pkg")"
        if command -v sha256sum >/dev/null 2>&1; then
            hash_val="$(sha256sum "$pkg" | awk '{print $1}')"
        elif command -v shasum >/dev/null 2>&1; then
            hash_val="$(shasum -a 256 "$pkg" | awk '{print $1}')"
        else
            hash_val="no_sha256_tool"
        fi
        printf "%s  %s\n" "$hash_val" "$pkg_name" >> "$CHECKSUM_FILE"
    fi
done

printf "\n${GREEN}========================================================${NC}\n"
printf "${GREEN} [ÉXITO] Release preparado en %s${NC}\n" "$RESOLVED_OUT_DIR"
printf "${GREEN}========================================================${NC}\n\n"

if [[ -f "$CHECKSUM_FILE" ]]; then
    printf "${CYAN}Firmas SHA-256 generadas:${NC}\n"
    cat "$CHECKSUM_FILE"
fi
printf "\n"
