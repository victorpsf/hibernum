using System.Data;
using NpgsqlTypes;

namespace Server.Models.Database;

public class Parameter
{
    public string Field { get; set; } = string.Empty;
    public object? Value { get; set; }
    public ParameterDirection Direction { get; set; }
    public NpgsqlDbType? Type { get; set; }
}