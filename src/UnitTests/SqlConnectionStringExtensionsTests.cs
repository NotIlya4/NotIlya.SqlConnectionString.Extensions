using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotIlya.SqlConnectionString.Extensions;

namespace UnitTests;

public class SqlConnectionStringExtensionsTests
{
    private readonly SqlConnectionStringBuilder _rawConn;
    private readonly SqlConnectionStringBuilder _explodedConn;
    private readonly IConfiguration _config;
    private const string RawConnKey = "SqlConnectionStringRaw";
    private const string ExplodedConnKey = "SqlConnectionStringExploded";

    public SqlConnectionStringExtensionsTests()
    {
        _rawConn = new SqlConnectionStringBuilder();
        _rawConn.DataSource = "localhost,666";
        _rawConn.InitialCatalog = "db";
        _rawConn.UserID = "root";
        _rawConn.Password = "pass";

        _explodedConn = new SqlConnectionStringBuilder();
        _explodedConn.DataSource = "localhost,123";
        _explodedConn.InitialCatalog = "dbb";
        _explodedConn.UserID = "roott";
        _explodedConn.Password = "passs";

        _config = new ConfigurationManager();
        _config[RawConnKey] = _rawConn.ConnectionString;
        _config[$"{ExplodedConnKey}:Data Source"] = _explodedConn.DataSource;
        _config[$"{ExplodedConnKey}:Initial Catalog"] = _explodedConn.InitialCatalog;
        _config[$"{ExplodedConnKey}:User Id"] = _explodedConn.UserID;
        _config[$"{ExplodedConnKey}:Password"] = _explodedConn.Password;
    }
    
    [Fact]
    public void GetSqlConnectionStringBuilder_ProvideConnUsingInlineString_ParseString()
    {
        SqlConnectionStringBuilder conn = GetRawConnFromConfig();
        
        Assert.Equal(_rawConn, conn);
    }

    [Fact]
    public void GetSqlConnectionStringBuilder_ProvideConnByExplodingItToProperties_ParseProperties()
    {
        SqlConnectionStringBuilder conn = GetExplodedConnFromConfig();
        
        Assert.Equal(_explodedConn, conn);
    }

    private SqlConnectionStringBuilder GetRawConnFromConfig()
    {
        return _config.GetSqlConnectionStringBuilder(RawConnKey);
    }

    private SqlConnectionStringBuilder GetExplodedConnFromConfig()
    {
        return _config.GetSqlConnectionStringBuilder(ExplodedConnKey);
    }
}