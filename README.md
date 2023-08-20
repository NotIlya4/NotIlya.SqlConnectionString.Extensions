# NotIlya.SqlConnectionString.Extensions [![NuGet Version](http://img.shields.io/nuget/v/NotIlya.SqlConnectionString.Extensions.svg?style=flat)]
Several extensions for `IConfiguration` that helps getting connection string.

## Quickstart
You can have config like this:
```json
{
  "SqlConnectionString": {
    "Server": "localhost,1433",
    "Database": "TestDb"
  }
}
```
Or like this:
```json
{
  "SqlConnectionString": "localhost,1433;Database=TestDb"
}
```
And you can get it using `config.GetSqlConnectionString()`. 

## Custom section
To specify your own section use `key` parameter in any method:
```json
{
  "SqlServer": {
    "Server": "localhost,1433",
    "Database": "TestDb"
  }
}
```
Get it by `config.GetSqlConnectionString("SqlServer")`.

## Development defaults
There is also method that has predefined defaults for development environments. Your empty config:
```json
{
  
}
```
Using this `config.GetDevelopmentSqlConnectionString()` empty config will be equivalent to:
```json
{
  "SqlConnectionString": {
    "Server": "localhost,1433",
    "User Id": "SA",
    "Password": "1tsJusT@S@mpleP@ssword!",
    "MultipleActiveResultSets": true,
    "TrustServerCertificate": true
  }
}
```
Of course you can override any of this values by providing them in config.

## SqlConnectionStringBuilder
My extensions built on top of `SqlConnectionStringBuilder` and instead of raw connection string you can get builder. Instead of `GetSqlConnectionString` use `GetSqlConnectionStringBuilder` and instead of `GetDevelopmentSqlConnectionString` use `GetDevelopmentSqlConnectionStringBuilder`.