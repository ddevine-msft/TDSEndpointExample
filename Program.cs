// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

SqlAuthenticationToken? _sqlAuthenticationToken = null;
string queryString = "select[Total Row Count] = count(*) from account";
string retVal = String.Empty;


// Build configuration to Retrieve the connection string from appsettings.json 
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
string connectionString = configuration["AppSettings:SQLConnectionString"];

try
{

    await using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    {
        sqlConnection.AccessTokenCallback = GetTokenCallback();
        var sqlCommand = new SqlCommand(queryString, sqlConnection);
        sqlCommand.CommandTimeout = 300;
        sqlConnection.Open();
        using (var reader = sqlCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                retVal = reader[0] != DBNull.Value ? reader[0]?.ToString() ?? "0" : "0";
                Console.WriteLine($"Total Row Count: {retVal}");
            }
        }
   }
    }
    catch (SqlException ex)
    {
    Console.WriteLine($"Error in ExecuteReader running query {queryString} - Exception: {ex} ");
    throw;
    }
    catch (Exception ex)
    {
    Console.WriteLine($"Error in ExecuteReader running query {queryString} - Exception: {ex} ");
    throw;
    }

//found this code on the internet here https://stackoverflow.com/questions/78325276/package-azure-identity-v1-11-0-and-above-caused-an-error-with-sqlclient-and-dyna
Func<SqlAuthenticationParameters, CancellationToken, Task<SqlAuthenticationToken?>> GetTokenCallback()
{
    var tenantId = configuration["AppSettings:TenantId"];
    var clientId = configuration["AppSettings:ClientId"];
    var clientSecret = configuration["AppSettings:ClientSecret"];
    const string defaultScopeSuffix = ".default";

    var cred = new ClientSecretCredential(tenantId, clientId, clientSecret);

    return TokenCallback;

    async Task<SqlAuthenticationToken?> TokenCallback(SqlAuthenticationParameters ctx, CancellationToken cancellationToken)
    {
        var validToken = _sqlAuthenticationToken != null && _sqlAuthenticationToken.ExpiresOn.UtcDateTime > DateTime.Now;
        if (validToken) return _sqlAuthenticationToken;

        string scope = ctx.Resource.EndsWith(defaultScopeSuffix)
            ? ctx.Resource
            : ctx.Resource + defaultScopeSuffix;

        var token = await cred.GetTokenAsync(new TokenRequestContext([scope]), CancellationToken.None);
        _sqlAuthenticationToken = new SqlAuthenticationToken(token.Token, token.ExpiresOn);
        return _sqlAuthenticationToken;
    }
}