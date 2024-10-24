namespace Server.Models.Database;

public class BancoExecuteScalarArgument
{
    public string Sql { get; set; } = string.Empty;
    public string Sequence { get; set; } = string.Empty;
    public ParameterCollection Parameter { get; set; } = new();
    public int CmdType { get; set; }
}