# Registro de Cambios (Changelog)

Todos los cambios notables en este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto se adhiere a [Versionado Semántico (SemVer)](https://semver.org/lang/es/).

---

## [Sin publicar]

---

## [1.0.0] - 2026-11-05

Lanzamiento oficial de la nueva plataforma **XPress PDF**, desarrollada y reescrita desde cero sobre **.NET 10** y **C# 14** con arquitectura limpia (*Clean Architecture*), motor de renderizado vectorial de alta precisión con **QuestPDF** y una interfaz de usuario multiplataforma moderna construida en **Avalonia UI 12**.

### Añadido

#### Motor de Deserialización XML CFDI 4.0 y Complementos (`Core.Xml`)
* **`CfdiXmlParser`**: Motor de lectura asíncrona segura con prohibición estricta de DTDs (`DtdProcessing.Prohibit`) y tolerancia a caracteres de espaciado y comentarios.
* **`XmlSerializerCache`**: Caché estática concurrente (`ConcurrentDictionary<Type, XmlSerializer>`) thread-safe que elimina el costo dinámico de recompilación de serializadores XML en conversiones recurrentes.
* **`CfdiComplementRegistry`**: Registro extensible de complementos fiscales que asocia espacios de nombres oficiales del SAT con tipos tipados de dominio sin modificar el núcleo del parser.
* **`CfdiComplementResolver`**: Deserializador y ensamblador modular que detecta e inyecta complementos a nivel comprobante (`<cfdi:Complemento>`) y a nivel concepto (`<cfdi:ComplementoConcepto>`).
* **Soporte de Complementos del SAT**:
  * *Timbre Fiscal Digital (TFD 1.1)*: Extracción de UUID, sello SAT, sello CFDI, fecha de timbrado y RFC del PAC.
  * *Nómina 1.2*: Entidades completas de emisor, receptor, percepciones (jubilación, indemnización, horas extra), deducciones, incapacidades y otros pagos (subsidio al empleo y saldos a favor).
  * *Recepción de Pagos 2.0*: Documentos relacionados, impuestos trasladados y retenidos por documento y resumen de totales fiscales.
  * *Vales de Despensa 1.0*: Conceptos de dispersión monetaria y beneficiarios con ordenamiento alfabético.
  * *Carta Porte 3.1*: Ubicaciones, mercancías, vehículos y figuras de transporte federal.
  * *Comercio Exterior 2.0*: Operaciones de exportación, certificados de origen y mercancías aduanales.
  * *Impuestos Locales 1.0*: Retenciones y traslados de impuestos estatales y municipales.
  * *Leyendas Fiscales 1.0*: Leyendas fiscales normativas.
  * *Divisas 1.0*: Operaciones de compra y venta de divisas.
  * *Instituciones Educativas (IEDU 1.0)*: Complemento a nivel concepto para colegiaturas y deducción fiscal de educación.

#### Motor de Representación Impresa PDF (`Core.Pdf`)
* **`CfdiPdfGenerator` y `CfdiPdfDocumentFactory`**: Orquestación y selección desacoplada de plantillas documentales QuestPDF según la naturaleza del CFDI.
* **Plantillas Documentales Especializadas**:
  * *`IngresoEgresoDocument`*: Representación impresa para facturas comerciales, recibos de arrendamiento, honorarios y notas de crédito.
  * *`NominaDocument`*: Formato balanceado para recibos de sueldo con igualación geométrica de altura entre las tablas de Percepciones y Deducciones, orden normativo de columnas (Exento \| Gravado) y tabla dedicada para Otros Pagos.
  * *`RecepcionPagosDocument`*: Desglose completo de pagos recibidos, documentos vinculados, saldos anteriores, importes pagados e insolutos.
  * *`TrasladoDocument`*: Representación gráfica para comprobantes de transporte de mercancías.
* **`IssuerRecipientComponent`**: Tarjetas de Datos del Emisor y Receptor con igualación de bordes exteriores e interlineado compacto nativo.
* **`TfdFooterComponent`**: Pie fiscal con código QR dinámico y responsivo, dimensionado proporcionalmente al área disponible, sellos digitales completos y cadena original del timbre fiscal digital.
* **`SatCatalogProvider`**: Catálogos normativos integrados del SAT para resolución de descripciones a partir de claves fiscales (Régimen Fiscal, Uso de CFDI, Forma de Pago, Método de Pago, Tipo de Contrato, Periodicidad, etc.).
* **`SatQrCodeGenerator` y `SatVerificationUrlBuilder`**: Generación de códigos QR oficiales con la URL de verificación del portal del SAT (`https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?...`).
* **`CurrencyToWordsConverter`**: Formateador en español mexicano de importes monetarios a texto formal (*«... PESOS 00/100 M.N.»*).

#### Interfaz Gráfica de Escritorio ERP (`AppUI`)
* **Ventana Frameless Moderna**: Diseño sin marco del sistema operativo con bordes redondeados (`CornerRadius="10"`), sombra perimetral de elevación, soporte de arrastre nativo (`PointerPressed`) y controles de ventana personalizados (Minimizar, Maximizar/Restaurar y Cerrar).
* **Sistema de Tokens de Diseño en Tres Niveles**:
  * *Primitivos*: Escalas cromáticas Blue, Indigo, Slate, Emerald, Amber, Rose y Alpha.
  * *Semánticos*: Tokens adaptables a temas Claro y Oscuro con conmutación en tiempo real (`ThemeService`).
  * *Componentes*: Dimensiones y espaciados estandarizados para tarjetas, entradas, botones y tablas.
* **Tipografía e Iconografía**: Integración de fuentes Fira Sans (Regular, Medium, SemiBold, Bold) y Fira Code, junto al catálogo vectorial completo de Lucide Icons embebido como geometrías XAML.
* **Espacio de Trabajo Masivo (`ConversionWorkspaceView`)**:
  * Zona unificada de arrastre y soltar (*Drag & Drop*) que se transforma en tabla interactiva de comprobantes.
  * Pre-escaneo asíncrono y concurrente de metadatos fiscales (UUID, tipo, RFCs, importes) controlado por semáforo para no degradar la interfaz.
  * Filtros en vivo por estado (Pendiente, Completado, Con Error) y por tipo de comprobante (Ingreso, Egreso, Pago, Nómina, Traslado).
  * KPIs en tiempo real con contadores de archivos totales, completados, pendientes y fallidos.
  * Barra de progreso responsiva con retroalimentación textual y porcentaje exacto.
  * Inspector lateral (*Drawer*) para consultar el detalle fiscal de cualquier CFDI en cola.
* **Bitácora de Diagnóstico (`DiagnosticsView`)**: Visualización virtualizada de eventos de conversión con filtrado por nivel de severidad (INFO, WARN, ERROR), búsqueda de texto libre y acceso directo a la carpeta de registros.
* **Preferencias Persistidas (`SettingsView`)**: Configuración a dos columnas con selección de tema (Sistema, Claro, Oscuro), carpeta de salida personalizada, carga y vista previa de logotipo empresarial, sobreescritura de archivos y persistencia en `%APPDATA%/XPressPDF/preferences.json`.

#### Identidad Visual Corporativa (`brand/`)
* **Sistema Normativo de Marca**: Rediseño geométrico del emblema *«Prisma de Conversión XPress»* sobre retícula técnica de 56 × 56 con la triada cromática oficial Azul 600 (`#2563EB`), Azul 900 (`#1E3A8A`) y Esmeralda 500 (`#10B981`).
* **Kit de 31 Activos Vectoriales SVG**: Generado mediante `build_brand.py` para las familias `isotipo/`, `imagotipo/`, `isologo/`, `logotipo/`, `favicon/`, `app-icon/` y `redes/`.
* **Manual de Marca Interactivo**: Documentación en `brand/marca.html` con las 12 secciones del sistema de diseño visual.
* **Iconos de Aplicación**: Generación de `app-icon.ico` multirresolución (16 a 256 px) y `app-icon.png` (512 × 512 px) e integración en ventana y barra de navegación.

#### Gobernanza, CI/CD y Empaquetado Multiplataforma
* **Aprovisionamiento GitFlow-Next**: Scripts `setup-gitflow.ps1` y `setup-gitflow.sh` para configuración de ramas base, fusiones `no-ff`, mensajes estandarizados `chore(merge): integrar %b en %p`, plantilla `.gitmessage` y hook local `.githooks/commit-msg`.
* **Scripts de Empaquetado Multiplataforma**: `build-release.ps1` y `build-release.sh` para compilar distribuciones *Standalone* y *Portable* para Windows, Linux y macOS en arquitecturas `x64` y `ARM64`.
* **Integración Continua (`ci.yml`)**: Workflow de GitHub Actions con validación automática de mensajes de commit según el manual v2.0.0 y matriz de compilación en Windows, Linux y macOS.
* **Publicación Oficial de Release (`release.yml`)**: Pipeline automatizado que compila en runners nativos los 12 paquetes de distribución, consolida firmas SHA-256 en `SHA256SUMS.txt`, extrae notas de versión de este archivo y publica la Release en GitHub.
