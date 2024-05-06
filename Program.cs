using System;
using System.Threading.Tasks;
using Azure.Identity;
using Npgsql;

class Program
{
 
    static async Task Main(string[] args)
    {

    string host = "umilogintest.postgres.database.azure.com";
    string database = "postgres";
    string managedIdentityClientId = "b1583b97-7a3a-46d4-953a-a10e856e0539"; // If using a user-assigned managed identity
    string userAssignedIdentities = "PGFS-LOGIN";

        var connectionString = $"Host={host}; Database={database}; SSL Mode=Require; Trust Server Certificate=true"; // Replace with your PostgreSQL connection string
        
        // Acquire token using User Managed Identity
        var tokenCredential = new ManagedIdentityCredential();
        var accessToken = await tokenCredential.GetTokenAsync(new Azure.Core.TokenRequestContext(new[] { "https://ossrdbms-aad.database.windows.net" }));

        // Connect to PostgreSQL using the acquired token
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connected to PostgreSQL database!");

        // Do your database operations here
        // Example: execute a query
        using var cmd = new NpgsqlCommand("SELECT * FROM your_table", connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(0));
        }
    }
}
