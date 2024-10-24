using System.Data;
using Dapper;
using Npgsql;
using Server.Models.Database;

namespace Server.Database.Connector.Client;

public class PostgresClient
{
    private readonly int CommandTimeOut = 30000;
    private readonly string ConnectionString;
    private NpgsqlConnection? Connection;
    private NpgsqlTransaction? Transaction;
    
    public PostgresClient(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    private NpgsqlConnection CreateConnection()
    { return new NpgsqlConnection(this.ConnectionString); }

    public void Connect()
    {
        this.Connection = this.CreateConnection();
        if (this.Connection.State == ConnectionState.Closed)
            this.Connection.Open();
        if (this.Transaction is null)
            this.Transaction = this.Connection.BeginTransaction();
    }

    public void Disconnect()
    {
        if (this.Connection?.State != ConnectionState.Closed)
            this.Connection?.Close();
        this.Connection = null;
        this.Transaction = null;
    }

    public void Commit()
    { this.Transaction?.Commit(); }

    public void Rollback()
    { this.Transaction?.Rollback(); }

    private NpgsqlCommand prepareCommand(string sql, ParameterCollection collection)
    {
        var command = new NpgsqlCommand(sql, this.Connection, this.Transaction);
        command.CommandTimeout = this.CommandTimeOut;

        foreach (var parameter in collection.Parameters)
        {
            var param = new NpgsqlParameter();

            if (parameter.Type is not null) param.NpgsqlDbType = parameter.Type ??  default;
            param.Value = parameter.Value ?? DBNull.Value;
            param.Direction = parameter.Direction;
            param.ParameterName = parameter.Field;

            command.Parameters.Add(param);
        }
        
        return command;
    }
    
    private void prepareCommandScalar(string sql, out NpgsqlCommand cmd, out NpgsqlParameter param)
    {
        cmd = new NpgsqlCommand(sql, this.Connection, this.Transaction);
        cmd.CommandTimeout = this.CommandTimeOut;

        param = new NpgsqlParameter<long>();
        param.ParameterName = "@id";
        param.Direction = ParameterDirection.Output;
        param.DbType = DbType.Int64;

        cmd.Parameters.Add(param);
    }

    private string? hasColumn(NpgsqlDataReader reader, string column)
    {
        for (int x = 0; x < reader.FieldCount; x++)
        {
            if (reader.GetName(x).ToUpperInvariant() == column.ToUpperInvariant())
                return reader.GetName(x);

            if (string.Join("", reader.GetName(x).Split("_")).ToUpperInvariant() == column.ToUpperInvariant())
                return reader.GetName(x);
        }

        return null;
    }

    public IEnumerable<T> ExecuteReader<T>(BancoArgument args) where T: class, new()
    {
        var command = this.prepareCommand(args.Sql, args.Parameter);
        var reader = command.ExecuteReader();
        var results = new List<T>();

        while (reader.Read())
        {
            var value = new T();
            var properties = value.GetType().GetProperties();

            foreach (var propertie in properties)
            {
                var column = this.hasColumn(reader, propertie.Name);
                if (column is null)
                    continue;
                
                try
                { propertie.SetValue(value, reader[column]); }

                catch
                {  }
            }
            
            results.Add(value);
        }
        reader.Close();
        
        return results;
    }

    public T? Find<T>(BancoArgument args) where T: class, new()
    {
        var result = this.ExecuteReader<T>(args: args);
        return result.FirstOrDefault();
    }

    public void Execute(BancoExecuteArgument args)
    {
        var command = this.prepareCommand(args.Sql, args.Parameter);
        command.ExecuteNonQuery();
    }

    public long? GetNextValue(BancoExecuteScalarArgument args)
    {
        this.prepareCommandScalar(
            $"select nextval('{args.Sequence}') as id",
            out NpgsqlCommand cmd,
            out NpgsqlParameter param
        );

        cmd.ExecuteNonQuery();

        try
        { return Convert.ToInt64(param.Value); }
        catch { }
        
        try
        { return (long) Convert.ToInt32(param.Value); }
        
        catch {}

        return null;
    }
    
    public long Execute(BancoExecuteScalarArgument args)
    {
        var id = this.GetNextValue(args);

        if (id is null)
            throw new Exception("ID IS NULL");

        args.Parameter.Add("@id", id);
        var command = this.prepareCommand(args.Sql, args.Parameter);
        command.ExecuteNonQuery();

        return id ?? default;
    }
}