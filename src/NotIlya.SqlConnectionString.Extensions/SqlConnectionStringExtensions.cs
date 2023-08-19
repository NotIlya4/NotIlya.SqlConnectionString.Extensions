using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotIlya.SqlConnectionString.Extensions;

public static class SqlConnectionStringExtensions
{
    public static SqlConnectionStringBuilder GetSqlConnectionStringBuilder(
        this IConfiguration config, string key = "SqlConnectionString")
    {
        var section = config.GetSection(key);
        var builder = new SqlConnectionStringBuilder();
        builder.FillFromConfig(section);

        return builder;
    }
    
    public static SqlConnectionStringBuilder GetDevelopmentSqlConnectionStringBuilder(
        this IConfiguration config, string key = "SqlConnectionString")
    {
        var section = config.GetSection(key);
        var builder = new SqlConnectionStringBuilder();
        builder.FillWithDevelopmentDefaults();
        builder.FillFromConfig(section);

        return builder;
    }

    public static string GetSqlConnectionString(this IConfiguration config, string key = "SqlConnectionString")
    {
        return config.GetSqlConnectionStringBuilder(key).ConnectionString;
    }
    
    public static string GetDevelopmentSqlConnectionString(this IConfiguration config, string key = "SqlConnectionString")
    {
        return config.GetDevelopmentSqlConnectionStringBuilder(key).ConnectionString;
    }

    #region InternalStaff

    internal static Dictionary<string, string> ToDict(this IConfiguration config)
    {
        return new Dictionary<string, string>(config.GetChildren().Select(s => new KeyValuePair<string, string>(s.Key, s.Value!)));
    }

    internal static void FillWithDevelopmentDefaults(this SqlConnectionStringBuilder builder)
    {
        builder.DataSource = "localhost,1433";
        builder.UserID = "SA";
        builder.Password = "1tsJusT@S@mpleP@ssword!";
        builder.MultipleActiveResultSets = true;
        builder.TrustServerCertificate = true;
    }

    internal static void FillFromConfig(this SqlConnectionStringBuilder builder, IConfigurationSection config)
    {
        if (config.Value is null)
        {
            foreach (KeyValuePair<string,string> parameter in config.ToDict())
            {
                builder.Add(parameter.Key, parameter.Value);
            }
        }
        else
        {
            builder.ConnectionString = config.GetRawConn();
        }
    }

    internal static string GetRawConn(this IConfigurationSection config)
    {
        var value = config.Value;

        if (value is null)
        {
            throw new InvalidOperationException("Connection string must be not null");
        }
        
        return value;
    }
    
    #endregion
}