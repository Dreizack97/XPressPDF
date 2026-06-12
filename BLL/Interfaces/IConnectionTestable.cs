namespace BLL.Interfaces
{
    /// <summary>Servicios cuya conexión puede probarse desde la configuración.</summary>
    public interface IConnectionTestable
    {
        Task<bool> ConnectAsync();
    }
}
