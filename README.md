# XPressPDF 🚀 Conversor de CFDI a PDF

**XPressPDF** es una herramienta de escritorio .NET moderna y multiplataforma que transforma tus archivos XML de CFDI mexicanos en elegantes PDFs listos para imprimir, ¡en segundos! Genera la representación impresa de recibos de nómina, facturas comerciales y dispersiones de vales de despensa conforme a los lineamientos del SAT.

### ✨ Funcionalidades

- ⚡ **Generación ultrarrápida y concurrente** de PDFs a partir de XML CFDI 4.0
- 🧾 **Soporta nómina, facturas (Ingreso/Egreso) y complemento de vales de despensa**
- 📁 **Selector de archivos múltiple y arrastrar y soltar**
- 🆙 **Sube tus PDF mediante FTP**
- 🎨 **Diseño PDF profesional** con código QR de verificación del SAT, sellos y cadena original
- 🖥️ **Interfaz Avalonia UI multiplataforma: Windows, macOS y Linux**
- 🔓 **Código abierto (Licencia MIT)**

---

## 🚀 Primeros Pasos

### Requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download)
- Windows, Linux o macOS
  - En Linux, QuestPDF/SkiaSharp puede requerir `libfontconfig1`

### Instalación

```bash
git clone https://github.com/Dreizack97/XPressPDF.git

cd XPressPDF

dotnet restore
dotnet build
```

### Uso Rápido

```bash
dotnet run --project AppUI.Avalonia
```

1. **Agregar archivos**: selecciona tus XML o arrástralos a la ventana
2. **Convertir a PDF**: los PDF se generan junto a cada XML (nombre = UUID del timbre)
3. **Subir PDF (FTP)**: opcionalmente sube los PDF generados a tu servidor
4. **Configuración**: credenciales de FTP y correo, con prueba de conexión

> ⚙️ La configuración y los logs se guardan en la carpeta de datos del usuario
> (`%APPDATA%/XPressPDF` en Windows, `~/Library/Application Support/XPressPDF` en macOS,
> `~/.config/XPressPDF` en Linux). Las credenciales se almacenan en JSON sin cifrar;
> protege esa carpeta adecuadamente.

---

## 🖼️ Ejemplo de Salida

![Ejemplo](https://github.com/Dreizack97/XPressPDF/blob/e5821a1b7d8983ce32ae2742af34ddba528d0870/Example.png)

- Datos de emisor y receptor con catálogos del SAT
- Conceptos, impuestos, totales e importe con letra
- Tablas de percepciones y deducciones (nómina) y dispersión por beneficiario (vales)
- UUID, sellos digitales, cadena original y código QR de verificación del SAT

---

## 📂 Estructura del Proyecto

- `AppUI.Avalonia`: interfaz gráfica multiplataforma (Avalonia 11, MVVM con CommunityToolkit, inyección de dependencias)
- `BLL`: lógica de negocio, generación de PDF con QuestPDF/QRCoder, servicios FTP/SMTP
- `Schemas`: clases generadas de los esquemas XSD del SAT (CFDI 4.0 y complementos)

---

## 🙌 Contribuye

¡Tus PRs y sugerencias son bienvenidos! Abre un [issue](https://github.com/Dreizack97/XPressPDF/issues) o manda tu mejora.

---

## ⚖️ Licencia

MIT — Gratis para uso personal y comercial.

---

## 🙏 Agradecimientos

- [Avalonia UI](https://avaloniaui.net/) — Interfaz multiplataforma
- [QuestPDF](https://www.questpdf.com/) — Motor PDF
- [QRCoder](https://github.com/codebude/QRCoder) — Generador QR
- [FluentFTP](https://github.com/robinrodricks/FluentFTP) — Cliente FTP
- [MailKit](https://mimekit.net/) — Cliente SMTP
- [SAT](https://www.sat.gob.mx/) — Estándares CFDI en México
