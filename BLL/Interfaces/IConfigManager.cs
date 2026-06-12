using BLL.Objects;

namespace BLL.Interfaces
{
    public interface IConfigManager
    {
        /// <summary>Configuración vigente; se carga (o crea con valores por defecto) en el primer acceso.</summary>
        AppConfig Current { get; }

        void Save(AppConfig config);

        void Reload();
    }
}
