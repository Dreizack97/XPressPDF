namespace BLL.Utilities
{
    /// <summary>
    /// Rutas de datos de la aplicación en el perfil del usuario. La carpeta de instalación
    /// puede ser de solo lectura (Program Files, /Applications, /usr), por lo que la
    /// configuración y los logs se almacenan en ApplicationData.
    /// </summary>
    public static class AppPaths
    {
        public static string DataDirectory { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XPressPDF");

        public static string ConfigFile { get; } = Path.Combine(DataDirectory, "config.json");

        public static string LogDirectory { get; } = Path.Combine(DataDirectory, "Logs");
    }
}
