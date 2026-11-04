# XPressPDF

Solución de software modular de alto rendimiento para el procesamiento y gestión de documentos PDF, construida sobre **.NET 10** y **Avalonia UI**, implementando Clean Architecture y las mejores prácticas de la industria.

---

## 🏛️ Arquitectura del Sistema

La solución está organizada bajo principios de bajo acoplamiento y alta cohesión:

- **`Core/`**: Capa de dominio y lógica de negocio pura, agnóstica a la infraestructura y la presentación.
- **`IoC/`**: Contenedor de Inversión de Control (IoC) y composición de dependencias.
- **`AppUI/`**: Aplicación de escritorio multiplataforma desarrollada con **Avalonia UI 12** y el patrón **MVVM** mediante `CommunityToolkit.Mvvm`.

---

## 📋 Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (versión 10.0.100 o superior).
- [Git](https://git-scm.com/) (versión 2.30 o superior).
- [git-flow-next](https://github.com/gittower/git-flow-next) (extensión de GitFlow en Go).

### Instalación de `git-flow-next`

- **Windows (WinGet):**
  ```powershell
  winget install GitTower.GitFlowNext
  ```

- **Windows (Scoop):**
  ```powershell
  scoop install git-flow-next
  ```

- **macOS / Linux (Homebrew):**
  ```bash
  brew install gittower/tap/git-flow-next
  ```

---

## 🚀 Configuración Inicial del Entorno

Tras clonar el repositorio desde GitHub, ejecute el script automatizado para aprovisionar las ramas de GitFlow, las políticas de fusión sin avance rápido (`--no-ff`), la plantilla de mensajes y el hook de validación local:

### En Windows (PowerShell):
```powershell
.\scripts\setup-gitflow.ps1
```

### En macOS / Linux (Bash):
```bash
chmod +x ./scripts/setup-gitflow.sh
./scripts/setup-gitflow.sh
```

---

## 🌿 Flujo de Trabajo (GitFlow)

El proyecto sigue estrictamente el modelo **GitFlow-Next** con las siguientes responsabilidades:

| Rama | Propósito | Prefijo / Nombre |
|---|---|---|
| `main` | Código en producción. Solo recibe fusiones de `release/` y `hotfix/`. | `main` |
| `develop` | Rama de integración de la siguiente versión. Base para nuevas ramas. | `develop` |
| `feature/` | Desarrollo de nuevas funcionalidades. Nace de `develop` y vuelve a `develop`. | `feature/<nombre>` |
| `bugfix/` | Corrección de defectos detectados antes de publicar. Nace de `develop` (o `release/`). | `bugfix/<nombre>` |
| `release/` | Preparación y estabilización de versión antes de pasar a producción. | `release/<version>` |
| `hotfix/` | Corrección urgente de defectos en producción. Nace de `main`. | `hotfix/<version>` |

---

## 📝 Convención de Mensajes de Commit

Los mensajes deben apegarse a **Conventional Commits 1.0.0** y las reglas internas de la organización (versión 2.0.0):

```text
<tipo>[(<ámbito>)][!]: <asunto en infinitivo y minúscula>

[cuerpo opcional explicando el porqué, ajustado a 72 columnas]

[pie opcional con referencias a tareas: Refs: PROJ-123 · Closes: #45]
```

### Tipos admitidos
`feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`.

### Reglas clave del asunto
1. Separador obligatorio `: ` tras el tipo o ámbito.
2. Verbo en **infinitivo** (`agregar`, `corregir`, `actualizar`, etc.).
3. **Minúscula inicial** en la primera palabra del asunto.
4. **Sin punto final**.
5. Longitud máxima de **72 caracteres**.

El repositorio cuenta con un hook en `.githooks/commit-msg` que valida automáticamente cada confirmación antes de ser registrada.

---

## 🛠️ Compilación y Ejecución

Compilar toda la solución:
```bash
dotnet build XPressPDF.slnx
```

Ejecutar la interfaz de usuario:
```bash
dotnet run --project AppUI/AppUI.csproj
```

---

## 📄 Licencia

Este proyecto se distribuye bajo los términos de la Licencia MIT. Consulte el archivo [LICENSE.txt](LICENSE.txt) para más información.
