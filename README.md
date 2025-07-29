# XPressPDF 🚀 Generador de Recibos de Nómina en PDF

**XPressPDF** es una herramienta de escritorio .NET moderna y multiplataforma que transforma tus archivos XML de nómina mexicana (CFDI de Nómina) en elegantes PDFs listos para imprimir, ¡en segundos! Incluye diseño profesional, código QR SAT, procesamiento por lotes y validación amigable.

### ✨ Funcionalidades

- ⚡ **Generación ultrarrápida** de PDFs a partir de XML de nómina CFDI
- 📁 **Soporte para archivos individuales y carpetas completas**
- 🆙 **Sube tus archivos mediante FTP**
- 🎨 **Diseño PDF moderno y profesional**, con soporte para logotipo y QR SAT
- 💻 **Interfaz fácil, con validaciones robustas**
- 🖥️ **Compatible con Windows, Linux y MacOS**
- 🔓 **Código abierto (Licencia MIT)**

---

## 🚀 Primeros Pasos

### Requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download)
- Windows, Linux o MacOS

### Instalación

```bash
git clone https://github.com/Dreizack97/XPressPDF.git

cd XPressPDF

dotnet restore
dotnet build
```

### Uso Rápido

```bash
dotnet run
```
- **1**: Procesar archivo XML individual
- **2**: Procesar todos los XML de una carpeta
- **3**: Salir

¡Tus PDFs aparecerán junto a los XML listos para imprimir o compartir!

---

## 🖼️ Ejemplo de Salida

![Ejemplo](https://github.com/Dreizack97/XPressPDF/blob/e5821a1b7d8983ce32ae2742af34ddba528d0870/Example.png)

- Datos de empresa y empleado
- Tablas de percepciones y deducciones
- Firmas digitales, UUID SAT
- Código QR para verificación SAT

---

## 📂 Estructura del Proyecto

- `AppUI`: Interfaz Gráfica de Usuario
- `BLL`: Utilidades para archivos XML, lógica de negocio y renderizado profesional con QuestPDF y QRCoder

---

## 🙌 Contribuye

¡Tus PRs y sugerencias son bienvenidos! Abre un [issue](https://github.com/Dreizack97/XPressPDF/issues) o manda tu mejora.

---

## ⚖️ Licencia

MIT — Gratis para uso personal y comercial.

---

## 🙏 Agradecimientos

- [QuestPDF](https://www.questpdf.com/) — Motor PDF
- [QRCoder](https://github.com/codebude/QRCoder) — Generador QR
- [FluentFTP](https://github.com/robinrodricks/FluentFTP) — Cliente FTP
- [MailKit](https://mimekit.net/) — Cliente SMTP
- [SAT](https://www.sat.gob.mx/) — Estándares CFDI de nómina en México
