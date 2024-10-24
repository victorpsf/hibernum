namespace Hibernum.Server.Services.Rules;

public class FindProductDescriptionRule
{
    public long? Id { get; set; }
    public string? Value { get; set; } = string.Empty;
    public long? Product { get; set; }
    public long[] Products { get; set; } = Array.Empty<long>();
}