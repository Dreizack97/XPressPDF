namespace Core.Xml;

using System.Collections.Concurrent;
using System.Xml.Serialization;

/// <summary>
/// Caché thread-safe de instancias XmlSerializer por tipo para evitar el costo de recompilación dinámica.
/// </summary>
public static class XmlSerializerCache
{
    private static readonly ConcurrentDictionary<Type, XmlSerializer> Serializers = new();

    public static XmlSerializer GetSerializer(Type type) =>
        Serializers.GetOrAdd(type, t => new XmlSerializer(t));

    public static XmlSerializer GetSerializer<T>() => GetSerializer(typeof(T));
}
