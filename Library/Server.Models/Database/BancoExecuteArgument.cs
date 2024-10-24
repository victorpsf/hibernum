namespace Server.Models.Database;

public class BancoExecuteArgument
{
    public string Sql { get; set; } = string.Empty;
    public ParameterCollection Parameter { get; set; } = new();
    public int CmdType { get; set; }
}