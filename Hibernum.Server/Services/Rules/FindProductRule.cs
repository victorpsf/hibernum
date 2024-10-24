namespace Hibernum.Server.Services.Rules;

public class FindProductRule
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Size { get; set; }
    public long? Group { get; set; }
    public List<long>? Groups { get; set; }
    public List<long> Products { get; set; }
}