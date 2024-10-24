using System.Data;

namespace Server.Models.Database;

public class BancoArgument
{
    public string Sql { get; set; } = string.Empty;
    public ParameterCollection Parameter { get; set; } = new ParameterCollection();
    public int CmdType { get; set; } = (int)CommandType.Text;
}