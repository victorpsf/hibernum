using System.Data;
using NpgsqlTypes;

namespace Server.Models.Database;

public class ParameterCollection
{
    public List<Parameter> Parameters { get; } = new();

    public static ParameterCollection GetInstance() => new();

    public ParameterCollection Add(string field, object? value)
        => Add(field, value, ParameterDirection.Input);
    public ParameterCollection Add(string field, object? value, ParameterDirection direction)
    {
        this.Parameters.Add(new Parameter { Field = field, Value = value, Direction = direction });
        return this;
    }
    
    public ParameterCollection Add(string field, object? value, ParameterDirection direction, NpgsqlDbType type)
    {
        this.Parameters.Add(new Parameter { Field = field, Value = value, Direction = direction, Type = type});
        return this;
    }
}