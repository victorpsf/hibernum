using Hibernum.Server.Services.Queries;
using Hibernum.Server.Services.Rules;
using Server.Database.Connector;
using Server.Database.Models;
using Server.Dims;
using Server.Exceptions;
using Server.Library;
using Server.Models.Database;
using File = Server.Database.Models.File;

namespace Hibernum.Server.Services;

public partial class FileService
{
    private ConnectorFactory _factory;
    
    public FileService(ConnectorFactory factory)
    { this._factory = factory; }

    public static FileManager GetManager()
    {
        var manager = FileManager.Create(FileManager.AppDataDir);

        if (!manager.ExistsDirectory("Hibernum"))
            manager.Mkdir("Hibernum");

        manager = FileManager.Create(FileManager.Concat(FileManager.AppDataDir, "Hibernum"));

        if (!manager.ExistsDirectory("images"))
            manager.Mkdir("images");
        
        return FileManager.Create(FileManager.Concat(FileManager.AppDataDir, "Hibernum", "images"));
    }
    
    public List<File> FindProducts(List<Product> products)
    {
        var results = new List<File>();

        var client = this._factory.getHibernumClient();
        try
        {
            client.Connect();
            this.Build(
                FileSql.FindFileByProductRelationSql,
                products,
                out string sql,
                out ParameterCollection parameterCollection
            );
            
            results.AddRange(
                client.ExecuteReader<File>(new BancoArgument()
                {
                    Sql = sql,
                    Parameter = parameterCollection
                })
            );
        }
        
        catch { }
        
        finally { client.Disconnect(); }

        return results;
    }

    public List<File> Find(FindFileRule rule)
    {
        this.Build(
            FileSql.SelectFileSql,
            rule,
            out string sql,
            out ParameterCollection collection
        );

        var client = this._factory.getHibernumClient();
        var files = new List<File>();

        try
        {
            client.Connect();
            files.AddRange(
                client.ExecuteReader<File>(new BancoArgument()
                {
                    Sql = sql,
                    Parameter = collection
                })
            );
        }

        catch
        { }

        finally
        { client.Disconnect(); }

        return files;
    }

    private File Update(File file)
    {
        var client = this._factory.getHibernumClient();

        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = FileSql.UpdateFileSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@path", file.Path)
                    .Add("@name", file.Name)
                    .Add("@mime", file.Mime)
                    .Add("@size", file.Size)
                    .Add("@last_modified", file.LastModified)
                    .Add("@id", file.Id)
            });
            client.Commit();
        }
        
        catch 
        { client.Rollback(); }

        finally
        { client.Disconnect(); }
        
        return file;
    }
    
    private File Insert(File file)
    {
        var client = this._factory.getHibernumClient();

        try
        {
            client.Connect();
            long id = client.Execute(new BancoExecuteScalarArgument()
            {
                Sql = FileSql.InsertFileSql,
                Sequence = FileSql.SequenceName,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@path", file.Path)
                    .Add("@name", file.Name)
                    .Add("@mime", file.Mime)
                    .Add("@size", file.Size)
                    .Add("@CreatedAt", DateTime.UtcNow)
                    .Add("@last_modified", file.LastModified)
            });
            file.Id = id;
            client.Commit();
        }
        
        catch 
        { client.Rollback(); }

        finally
        { client.Disconnect(); }
        
        return file;
    }

    public File Save(FileDim dim)
    {
        var manager = GetManager();
        var path = manager.WriteFile(dim.Name ?? string.Empty, dim.Bytes.ToArray());
        
        if (dim.Id is not null)
        {
            var file = this.Find(new FindFileRule() { Id = dim.Id }).FirstOrDefault();
            if (file is null)
                throw new Exception();
        
            FileManager.Create(file.Path)
                .RmFile();
        
            return this.Update(new File()
            {
                Id = file.Id,
                Name = dim.Name ?? string.Empty,
                Mime = dim.Mime ?? string.Empty,
                Size = dim.Size ?? 0,
                Path = path,
                LastModified = dim.LastModified ?? DateTime.UtcNow
            });
        }
        
        return this.Insert(new File()
        {
            Name = dim.Name ?? string.Empty,
            Mime = dim.Mime ?? string.Empty,
            Size = dim.Size ?? 0,
            Path = path,
            LastModified = dim.LastModified ?? DateTime.UtcNow
        });
    }
    
    public File Remove(long id)
    {
        var result = this.Find(new FindFileRule() { Id = id }).FirstOrDefault();

        if (result is null)
            throw new BusinessException(BusinessExceptionEnum.FILE_NOT_EXISTS);
        
        var client = this._factory.getHibernumClient();
        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = FileSql.RemoveFileSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@id", result.Id)
                    .Add("@date", DateTime.UtcNow)
            });
            client.Commit();
        }

        catch
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        { client.Disconnect(); }

        return result;
    }
}