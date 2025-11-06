using Npgsql;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Testing PostgreSQL connection...");

var connectionString = "Host=db.bahreqhuivjicyhygivv.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=TechAds@_0511;SSL Mode=Require;Trust Server Certificate=true";

try
{
    using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    Console.WriteLine("✅ Connection successful!");

    // Test a simple query
    using var command = new NpgsqlCommand("SELECT version()", connection);
    var version = await command.ExecuteScalarAsync();
    Console.WriteLine($"PostgreSQL version: {version}");

    // Check if our tables exist
    using var tableCommand = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'", connection);
    using var reader = await tableCommand.ExecuteReaderAsync();
    Console.WriteLine("Tables in database:");
    while (await reader.ReadAsync())
    {
        Console.WriteLine($"- {reader.GetString(0)}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Connection failed: {ex.Message}");
}
