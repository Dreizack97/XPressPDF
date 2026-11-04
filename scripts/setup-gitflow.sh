#!/usr/bin/env bash
#
# Configura el flujo de trabajo GitFlow-Next, plantilla de confirmación y hooks locales.
#

set -euo pipefail

# Colores para salida de consola
CYAN='\033[0;36m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
MAGENTA='\033[0;35m'
NC='\033[0m' # No Color

step() {
    printf "\n${CYAN}[+] %s${NC}\n" "$1"
}

success() {
    printf "    ${GREEN}[OK] %s${NC}\n" "$1"
}

warning() {
    printf "    ${YELLOW}[!] %s${NC}\n" "$1"
}

error() {
    printf "\n${RED}[ERROR] %s${NC}\n" "$1" >&2
}

printf "${MAGENTA}============================================================${NC}\n"
printf "${MAGENTA}  XPressPDF - Configuración de GitFlow-Next y Convenciones  ${NC}\n"
printf "${MAGENTA}============================================================${NC}\n"

# 1. Comprobar entorno Git y situarse en la raíz del repositorio
step "Comprobando entorno de Git..."
if ! repo_root="$(git rev-parse --show-toplevel 2>/dev/null)"; then
    error "El directorio actual no es un repositorio Git válido."
    exit 1
fi
cd "$repo_root"
success "Raíz del repositorio detectada: $repo_root"

# 2. Comprobar presencia de git-flow-next
step "Verificando extensión git-flow-next..."
if ! git_flow_ver="$(git flow version 2>/dev/null)"; then
    error "La extensión git-flow-next no está instalada o no se encuentra en el PATH."
    printf "${YELLOW}\nPara instalarla en macOS / Linux (Homebrew):${NC}\n"
    printf "  brew install gittower/tap/git-flow-next\n\n"
    printf "${YELLOW}En Windows (WinGet / Scoop):${NC}\n"
    printf "  winget install GitTower.GitFlowNext\n"
    printf "  scoop install git-flow-next\n\n"
    exit 1
fi
success "git-flow-next detectado: $git_flow_ver"

# 3. Asegurar ramas base locales (main y develop)
step "Asegurando ramas base (main y develop)..."

if ! git show-ref --verify --quiet refs/heads/main; then
    if git show-ref --verify --quiet refs/remotes/origin/main; then
        git branch --track main origin/main >/dev/null
        success "Rama 'main' vinculada con origin/main."
    else
        current_branch="$(git branch --show-current)"
        if [ "$current_branch" != "main" ]; then
            git branch -M main >/dev/null
        fi
        success "Rama local 'main' establecida."
    fi
else
    success "Rama local 'main' disponible."
fi

if ! git show-ref --verify --quiet refs/heads/develop; then
    if git show-ref --verify --quiet refs/remotes/origin/develop; then
        git branch --track develop origin/develop >/dev/null
        success "Rama 'develop' vinculada con origin/develop."
    else
        git branch develop main >/dev/null
        success "Rama local 'develop' creada a partir de 'main'."
    fi
else
    success "Rama local 'develop' disponible."
fi

# 4. Inicializar GitFlow-Next
step "Inicializando configuración de GitFlow-Next..."
git flow init -f --preset=classic \
    --main main \
    --develop develop \
    --feature "feature/" \
    --bugfix "bugfix/" \
    --release "release/" \
    --hotfix "hotfix/" \
    --support "support/" \
    --tag "v" >/dev/null
success "Prefijos de ramas y ramas base inicializadas con éxito."

# 5. Configurar fusiones sin avance rápido (--no-ff)
step "Configurando fusiones sin avance rápido (--no-ff)..."
for type in feature bugfix release hotfix support; do
    git config "gitflow.$type.finish.no-ff" true
done
success "Fusiones no-ff forzadas para feature, bugfix, release, hotfix y support."

# 6. Configurar mensajes de fusión según Conventional Commits v2.0.0
step "Configurando mensajes estandarizados de fusión (mergemessage / updatemessage)..."
merge_msg="chore(merge): integrar %b en %p"

for type in feature bugfix release hotfix support; do
    git config "gitflow.$type.finish.mergemessage" "$merge_msg"
done

git config "gitflow.release.finish.updatemessage" "$merge_msg"
git config "gitflow.hotfix.finish.updatemessage" "$merge_msg"
success "Mensajes de fusión configurados con formato: '$merge_msg'."

# 7. Configurar plantilla de mensajes de commit
step "Activando plantilla de confirmación (.gitmessage)..."
if [ -f ".gitmessage" ]; then
    git config commit.template .gitmessage
    success "Plantilla commit.template asociada a .gitmessage."
else
    warning "No se encontró el archivo .gitmessage en la raíz."
fi

# 8. Configurar ruta de hooks locales y permisos
step "Activando hooks de validación locales (.githooks)..."
if [ -f ".githooks/commit-msg" ]; then
    chmod +x .githooks/commit-msg 2>/dev/null || true
    git config core.hooksPath .githooks
    success "core.hooksPath establecido en .githooks y permisos asignados."
else
    warning "No se encontró el hook .githooks/commit-msg."
fi

# 9. Cambiar a rama develop
step "Cambiando a la rama de integración 'develop'..."
git switch develop >/dev/null
success "Rama activa actual: $(git branch --show-current)"

# 10. Resumen final
step "Verificando parámetros aplicados en el repositorio..."
keys_count=$(git config --get-regexp '^gitflow\..*finish' | wc -l | tr -d ' ')
template_key=$(git config commit.template || echo "no configurado")
hooks_key=$(git config core.hooksPath || echo "no configurado")

printf "\n------------------- Resumen de Configuración -------------------\n"
printf "Ramas base:          main (producción), develop (integración)\n"
printf "Prefijos:            feature/, bugfix/, release/, hotfix/, support/\n"
printf "Prefijo de versión:  v (SemVer)\n"
printf "commit.template:     %s\n" "$template_key"
printf "core.hooksPath:      %s\n" "$hooks_key"
printf "Claves de cierre:    %s claves 'finish' configuradas\n" "$keys_count"
printf "----------------------------------------------------------------\n"

printf "\n${GREEN}[✓] Repositorio configurado exitosamente según las políticas internas.${NC}\n\n"
