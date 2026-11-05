# XPress PDF

<p align="center">
  <img src="brand/imagotipo/imagotipo-horizontal-color.svg" alt="XPress PDF Logotipo" width="360" />
</p>

<p align="center">
  <strong>El conversor masivo y de alto rendimiento de comprobantes fiscales CFDI 4.0 a documentos PDF elegantes y normativos.</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 10" />
  <img src="https://img.shields.io/badge/C%23-14-239120?style=flat-square&logo=csharp&logoColor=white" alt="C# 14" />
  <img src="https://img.shields.io/badge/UI-Avalonia%2012-8A2BE2?style=flat-square&logo=avalonia&logoColor=white" alt="Avalonia UI 12" />
  <img src="https://img.shields.io/badge/PDF-QuestPDF-0078D7?style=flat-square" alt="QuestPDF" />
  <img src="https://img.shields.io/badge/Versi%C3%B3n-1.0.0-blue.svg?style=flat-square" alt="Versión 1.0.0" />
  <img src="https://img.shields.io/badge/Plataformas-Windows%20%7C%20Linux%20%7C%20macOS-informational?style=flat-square" alt="Plataformas" />
  <img src="https://img.shields.io/badge/Licencia-MIT-green.svg?style=flat-square" alt="Licencia MIT" />
</p>

---

## 🎯 ¿Qué es XPress PDF?

**XPress PDF** es una aplicación de escritorio multiplataforma concebida para despachos contables, empresas, departamentos de nómina y contribuyentes en México que necesitan convertir cientos o miles de archivos XML de **CFDI 4.0** a representaciones impresas en PDF de máxima calidad visual en cuestión de segundos.

Construida con la potencia de **.NET 10**, **C# 14** y **QuestPDF**, genera documentos vectoriales nítidos con código QR oficial del SAT, sellos digitales, cadena original, desglose analítico de impuestos e importe formal con letra, manteniendo la información fiscal 100 % privada y procesada de forma local en tu equipo.

---

## 🧾 Comprobantes y Complementos Soportados

XPress PDF reconoce y procesa automáticamente los esquemas oficiales vigentes del SAT:

| Tipo de Comprobante | Clave | Casos de Uso Habituales |
|---|:---:|---|
| **Ingreso** | `I` | Facturas comerciales, recibos de honorarios y de arrendamiento. |
| **Egreso** | `E` | Notas de crédito, devoluciones y bonificaciones. |
| **Nómina** | `N` | Recibos de sueldo ordinarios y extraordinarios con detalle patronal y laboral. |
| **Recepción de Pagos** | `P` | Recibos electrónicos de pago (REP) con documentos relacionados e impuestos. |
| **Traslado** | `T` | Comprobantes de transporte y movilización de mercancías. |

### Complementos Fiscales Integrados
* **Timbre Fiscal Digital (TFD 1.1)**: Código QR responsivo, sellos digitales y UUID.
* **Nómina 1.2**: Tablas de Percepciones y Deducciones con balance visual, incapacidades y otros pagos.
* **Pagos 2.0**: Desglose por documento relacionado, parcialidades e impuestos de pago.
* **Carta Porte 3.1**: Ubicaciones de origen/destino, mercancías y vehículos federales.
* **Vales de Despensa 1.0**: Relación monetaria y beneficiarios en orden alfabético.
* **Comercio Exterior 2.0**: Claves de pedimento, certificados y valor en aduana.
* **Impuestos Locales 1.0**: Retenciones y traslados estatales o municipales (ej. ISH).
* **Leyendas Fiscales 1.0**: Disposiciones fiscales y decretos aplicables.
* **Divisas 1.0**: Operaciones cambiarias de compra/venta de moneda extranjera.
* **Instituciones Educativas (IEDU 1.0)**: Deducción de colegiaturas a nivel concepto.

---

## ✨ Características Principales

* ⚡ **Procesamiento Concurrente de Alto Rendimiento**: Orquestación multihilo con `Parallel.ForEachAsync` que aprovecha todos los núcleos de tu procesador para convertir lotes masivos sin congelar la interfaz.
* 🎨 **Diseño PDF Vectorial y Balanceado**: Tablas de percepciones y deducciones de nómina con igualación simétrica de altura, tarjetas de emisor y receptor compactas y código QR con dimensionamiento responsivo.
* 🔍 **Pre-escaneo Fiscal Asíncrono**: Lectura en segundo plano de UUID, tipo, RFCs e importes al agregar archivos, facilitando la búsqueda y filtrado inmediato.
* 🖼️ **Personalización Corporativa**: Incorpora el logotipo oficial de tu empresa en los encabezados de todos los comprobantes emitidos.
* 🔒 **Privacidad Total**: No depende de servicios en la nube ni almacena credenciales de terceros; tus comprobantes nunca salen de tu ordenador.
* 🖥️ **Interfaz Moderna y Accesible**: Ventana sin marco con bordes redondeados, soporte nativo de arrastrar y soltar (*drag & drop*), inspector lateral de comprobantes y temas Claro, Oscuro y de Sistema.

---

## 📦 Descargas Oficiales por Sistema Operativo

En la sección de [Releases de GitHub](https://github.com/Dreizack97/XPressPDF/releases) encontrarás los paquetes oficiales para tu plataforma:

| Sistema Operativo | Arquitectura | Modalidad Standalone (Recomendada) | Modalidad Portable |
|---|---|---|---|
| **Windows 10 / 11** | x64 (Intel / AMD 64-bit) | `XPressPDF-v1.0.0-win-x64-standalone.zip` | `XPressPDF-v1.0.0-win-x64-portable.zip` |
| **Windows 10 / 11** | ARM64 (Snapdragon / Copilot+) | `XPressPDF-v1.0.0-win-arm64-standalone.zip` | `XPressPDF-v1.0.0-win-arm64-portable.zip` |
| **Linux** | x64 (Ubuntu, Debian, Fedora, Arch) | `XPressPDF-v1.0.0-linux-x64-standalone.tar.gz` | `XPressPDF-v1.0.0-linux-x64-portable.tar.gz` |
| **Linux** | ARM64 (Raspberry Pi 4/5, Servidores) | `XPressPDF-v1.0.0-linux-arm64-standalone.tar.gz` | `XPressPDF-v1.0.0-linux-arm64-portable.tar.gz` |
| **macOS** | Apple Silicon (M1 / M2 / M3 / M4) | `XPressPDF-v1.0.0-osx-arm64-standalone.tar.gz` | `XPressPDF-v1.0.0-osx-arm64-portable.tar.gz` |
| **macOS** | Intel x64 | `XPressPDF-v1.0.0-osx-x64-standalone.tar.gz` | `XPressPDF-v1.0.0-osx-x64-portable.tar.gz` |

### ¿Qué modalidad elegir?
* **Standalone (Recomendada)**: Incluye el motor de .NET 10 compilado internamente. **No necesitas instalar nada previo**. Descomprime y ejecuta directamente el programa.
* **Portable**: Edición de peso reducido para desarrolladores o usuarios que ya disponen del [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) en su sistema operativo.

### Requisitos Adicionales por Sistema Operativo
* **En Linux**: El motor gráfico SkiaSharp / QuestPDF requiere las bibliotecas de fuentes del sistema:
  ```bash
  # Debian / Ubuntu / Mint:
  sudo apt install libfontconfig1

  # Fedora / RHEL:
  sudo dnf install fontconfig

  # Arch Linux:
  sudo pacman -S fontconfig
  ```
* **En macOS**: Si el sistema bloquea la apertura inicial de un binario descargado, autorízalo en *Ajustes del Sistema > Privacidad y Seguridad*, o retira la marca de cuarentena mediante:
  ```bash
  xattr -d com.apple.quarantine XPressPDF
  ```

---

## 🚀 Guía de Uso Rápido

1. **Cargar comprobantes**: Arrastra archivos XML directamente a la ventana principal, o pulsa **`Cargar Archivos XML`** para seleccionarlos mediante el explorador.
2. **Revisar e inspeccionar**: Observa los KPIs en tiempo real (totales, pendientes, completados). Haz clic en cualquier comprobante para abrir el panel lateral con su desglose fiscal completo (UUID, emisor, receptor, tipo e importes).
3. **Convertir a PDF**: Pulsa **`Convertir Todo`** para procesar todo el lote en paralelo, o genera el PDF de una factura específica haciendo clic en el icono de acción de su fila.
4. **Configurar preferencias**: Desde el menú de **Configuración**, define una carpeta de salida predeterminada, selecciona tu tema visual predilecto (Claro/Oscuro) o sube el logotipo corporativo de tu empresa.

---

## 🛠️ Compilación y Empaquetado desde el Código Fuente

### Requisitos de Desarrollo
* [.NET 10 SDK](https://dotnet.microsoft.com/download) (10.0.100 o superior).
* [Git](https://git-scm.com/) y la extensión [git-flow-next](https://github.com/gittower/git-flow-next).

### 1. Clonar y Configurar Entorno GitFlow
```bash
git clone https://github.com/Dreizack97/XPressPDF.git
cd XPressPDF
```

Ejecuta el asistente automatizado para configurar las políticas de ramas y validación local:
```powershell
# En Windows (PowerShell)
.\scripts\setup-gitflow.ps1
```
```bash
# En macOS o Linux (Bash)
chmod +x ./scripts/setup-gitflow.sh
./scripts/setup-gitflow.sh
```

### 2. Compilar y Ejecutar en Desarrollo
```bash
# Restaurar dependencias
dotnet restore XPressPDF.slnx

# Compilar solución completa
dotnet build XPressPDF.slnx -c Release

# Ejecutar aplicación de escritorio
dotnet run --project AppUI/AppUI.csproj -c Release
```

### 3. Generar Paquetes de Distribución Oficiales
Para compilar y empaquetar de forma automatizada las ediciones *Standalone* y *Portable* con sus sumas de comprobación SHA-256:

```powershell
# En Windows (PowerShell): compilar para win-x64 o todas las plataformas
.\scripts\build-release.ps1 -Version "1.0.0" -Runtime "win-x64" -Clean
```

```bash
# En Linux o macOS (Bash):
chmod +x ./scripts/build-release.sh
./scripts/build-release.sh --version "1.0.0" --runtime "linux-x64" --clean
```

---

## 🏛️ Arquitectura de la Solución

XPress PDF sigue una arquitectura limpia de tres niveles:

* **`Core/`**: Dominio y lógica de negocio pura. Contiene los modelos fiscales de CFDI 4.0, el registro y resolución de complementos (`CfdiComplementRegistry`), catálogos normativos del SAT y el motor de generación documental con QuestPDF. No depende de ningún componente de interfaz de usuario.
* **`IoC/`**: Contenedor de Inversión de Control para composición modular del motor mediante `AddXPressPdfCore()`.
* **`AppUI/`**: Aplicación de escritorio moderna construida en **Avalonia UI 12** implementando el patrón MVVM (`CommunityToolkit.Mvvm`), ventana *frameless*, tokens de diseño en tres niveles e iconografía Lucide.

Consulte el [Manual de Identidad Visual](brand/marca.html) en `brand/` para conocer la retícula técnica y normativas de uso de la marca.

---

## 🤝 Comunidad y Contribución

¡Las contribuciones son bienvenidas! Te invitamos a consultar:
* La [Guía de Contribución](CONTRIBUTING.md) para conocer las normas de GitFlow-Next y la convención de mensajes Conventional Commits (Manual v2.0.0).
* El [Código de Conducta](CODE_OF_CONDUCT.md) para garantizar un ambiente colaborativo y respetuoso.
* El [Registro de Cambios (CHANGELOG.md)](CHANGELOG.md) para consultar el historial detallado de versiones.

---

## 📄 Licencia

Este proyecto está distribuido bajo la licencia **MIT**. Consulte el archivo [LICENSE.txt](LICENSE.txt) para más información.
