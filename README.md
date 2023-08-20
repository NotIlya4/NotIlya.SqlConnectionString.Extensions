# NotIlya.SqlConnectionString.Extensions
Several extensions for `IConfiguration` that helps getting connection string.

## Use cases
### Explode connection string
Config:
```json
{
  "SqlConnectionString": {
    "Server": "localhost,1433", // or "Data Source"
    "Database": "TestDb", // or "Initial Catalog"
    "User Id": "SA",
    "Password": "1tsJusT@S@mpleP@ssword!"
  }
}
```
Result:
```csharp
var connectionString = config.GetSqlConnectionString();
/* connectionString == "localhost,1433;
                        Initial Catalog=TestDb;
                        User Id=SA;
                        Password=1tsJusT@S@mpleP@ssword!" 
*/
```
**P.S.:** You can specify your own section using `key` parameter in any method:
```json
{
  "SqlServer": { // Edited section
    "Server": "localhost,1433",
    "Database": "TestDb"
  }
}
```
Get it:
```csharp
var connectionString = config.GetSqlConnectionString("SqlServer");
```

### Inline connection string
You can inline connection string:
```json
{
  "SqlConnectionString": "localhost,1433;Initial Catalog=TestDb;User Id=SA;Password=1tsJusT@S@mpleP@ssword!"
}
```
Result:
```csharp
var connectionString = config.GetSqlConnectionString();
/* connectionString == "localhost,1433;
                        Initial Catalog=TestDb;
                        User Id=SA;
                        Password=1tsJusT@S@mpleP@ssword!"
*/
```

### Defaults for development environment
Config:
```json
{
  // Empty. But you can override any development defaults
}
```
Result:
```csharp
var connectionString = config.GetDevelopmentSqlConnectionString();
/* connectionString == "localhost,1433;
                        User Id=SA;
                        Password=1tsJusT@S@mpleP@ssword!;
                        MultipleActiveResultSets=True;
                        TrustServerCertificate=True"
*/
```