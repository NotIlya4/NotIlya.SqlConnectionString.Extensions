using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotIlya.SqlConnectionString.Extensions;

namespace UnitTests;

public class SqlConnectionStringExtensionsTests
{
    private readonly SqlConnectionStringBuilder _rawConn;
    private readonly SqlConnectionStringBuilder _explodedConn;
    private readonly IConfiguration _rawConnConfig;
    private readonly IConfiguration _explodedConnConfig;

    public SqlConnectionStringExtensionsTests()
    {
        _rawConn = new SqlConnectionStringBuilder();
        _rawConn.DataSource = "localhost,666";
        _rawConn.InitialCatalog = "db";
        _rawConn.UserID = "root";
        _rawConn.Password = "pass";

        _rawConnConfig = new ConfigurationManager()
        {
            ["Sql"] = _rawConn.ConnectionString
        };

        _explodedConn = new SqlConnectionStringBuilder();
        _explodedConn.DataSource = "localhost,123";
        _explodedConn.InitialCatalog = "dbb";
        _explodedConn.UserID = "roott";
        _explodedConn.Password = "passs";

        _explodedConnConfig = new ConfigurationManager()
        {
            ["Sql:Data Source"] = _explodedConn.DataSource,
            ["Sql:Initial Catalog"] = _explodedConn.InitialCatalog,
            ["Sql:User Id"] = _explodedConn.UserID,
            ["Sql:Password"] = _explodedConn.Password
        };
    }
    
    [Fact]
    public void GetSqlConnectionStringBuilder_ProvideConnUsingInlineString_ParseString()
    {
        SqlConnectionStringBuilder conn = _rawConnConfig.GetSqlConnectionStringBuilder("Sql");
        
        Assert.Equal(_rawConn, conn);
    }

    [Fact]
    public void GetSqlConnectionStringBuilder_ProvideConnByExplodingItToProperties_ParseProperties()
    {
        SqlConnectionStringBuilder conn = _explodedConnConfig.GetSqlConnectionStringBuilder("Sql");
        
        Assert.Equal(_explodedConn, conn);
    }
}