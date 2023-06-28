using Globomantics.Domain;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globomantics.Windows.Json;

public class SerializationBinder : ISerializationBinder
{
    private static readonly DefaultSerializationBinder Binder 
        = new DefaultSerializationBinder();

    private IList<string> AllowedTypes { get; }

    public SerializationBinder()
    {
        AllowedTypes = typeof(Todo)
        .Assembly
        .GetTypes()
        .Where(type => type.IsClass && type.Namespace == typeof(Todo).Namespace)
        .Select(type => type.FullName ?? "")
        .ToList();

        AllowedTypes.Add("System.Byte[][]");
        AllowedTypes.Add(typeof(TodoTask[]).FullName ?? "");
    }

    public void BindToName(Type serializedType,
        out string? assemblyName,
        out string? typeName)
    {
        assemblyName = serializedType.Assembly.FullName;
        typeName = serializedType.FullName;
    }

    public Type BindToType(string? assemblyName, string typeName)
    {
        if (!AllowedTypes.Contains(typeName)) return null!;

        return Binder.BindToType(assemblyName, typeName);
    }
}