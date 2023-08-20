using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotIlya.SqlConnectionString.Extensions;

/// <summary>
/// Extends <see cref="IConfiguration"/> with methods for getting sql connection string.
/// </summary>
public static class SqlConnectionStringExtensions
{
    /// <summary>
    /// Gets connection string builder from specified key. It can be either raw connection string
    /// or connection string exploded to parameters.
    /// </summary>
    /// <param name="config">Source of configuration</param>
    /// <param name="key">Section that will be used for connection string</param>
    /// <returns>Sql connection string builder for section</returns>
    public static SqlConnectionStringBuilder GetSqlConnectionStringBuilder(
        this IConfiguration config, string key = "SqlConnectionString")
    {
        var section = config.GetSection(key);
        var builder = new SqlConnectionStringBuilder();
        builder.FillFromConfig(section);

        return builder;
    }
    
    /// <summary>
    /// Gets connection string builder from specified key. Adds some default values that common for
    /// development environment:
    /// "Data Source=localhost,1433;User Id=SA;Password=1tsJusT@S@mpleP@ssword!;MultipleActiveResultSets=true;TrustServerCertificate=true".
    /// It can be either raw connection string or connection string exploded to parameters.
    /// </summary>
    /// <param name="config">Source of configuration</param>
    /// <param name="key">Section that will be used for connection string</param>
    /// <returns>Sql connection string builder for section</returns>
    public static SqlConnectionStringBuilder GetDevelopmentSqlConnectionStringBuilder(
        this IConfiguration config, string key = "SqlConnectionString")
    {
        var section = config.GetSection(key);
        var builder = new SqlConnectionStringBuilder();
        builder.FillWithDevelopmentDefaults();
        builder.FillFromConfig(section);

        return builder;
    }

    /// <summary>
    /// Gets connection string from specified key. It can be either raw connection string
    /// or connection string exploded to parameters.
    /// </summary>
    /// <param name="config">Source of configuration</param>
    /// <param name="key">Section that will be used for connection string</param>
    /// <returns>Parsed sql connection string for section</returns>
    public static string GetSqlConnectionString(this IConfiguration config, string key = "SqlConnectionString")
    {
        return config.GetSqlConnectionStringBuilder(key).ConnectionString;
    }
    
    /// <summary>
    /// Gets connection string from specified key. Adds some default values that common for
    /// development environment:
    /// "Data Source=localhost,1433;User Id=SA;Password=1tsJusT@S@mpleP@ssword!;MultipleActiveResultSets=true;TrustServerCertificate=true".
    /// It can be either raw connection string or connection string exploded to parameters.
    /// </summary>
    /// <param name="config">Source of configuration</param>
    /// <param name="key">Section that will be used for connection string</param>
    /// <returns>Parsed sql connection string for section</returns>
    public static string GetDevelopmentSqlConnectionString(this IConfiguration config, string key = "SqlConnectionString")
    {
        return config.GetDevelopmentSqlConnectionStringBuilder(key).ConnectionString;
    }

    #region InternalStaff

    internal static Dictionary<string, string?> ToDict(this IConfiguration config)
    {
        return config.GetChildren().ToDictionary(k => k.Key, v => v.Value);
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
            foreach (KeyValuePair<string,string?> parameter in config.ToDict())
            {
                builder.Add(parameter.Key, parameter.Value ?? "");
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