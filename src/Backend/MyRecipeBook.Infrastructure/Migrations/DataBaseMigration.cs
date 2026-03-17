using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        // 1. Primeiro, garante que o banco de dados 'MyRecipeBookDb' existe.
        EnsureDatabaseExists(connectionString);

        // 2. (Passo Futuro): Aqui entrará a execução das suas migrações de tabelas.
        // Se o seu professor usar o FluentMigrator para criar as tabelas, o código de execução dele virá logo abaixo desta linha.
        MigrationDataBase(serviceProvider);
    }

    private static void EnsureDatabaseExists(string connectionString)
    {
        // 1. Extrai o nome do banco de dados que você quer criar
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;
    
        // 2. Muda a conexão temporariamente para o banco padrão do sistema ('postgres')
        builder.Database = "postgres"; 
    
        using var dbConnection = new NpgsqlConnection(builder.ToString());
        dbConnection.Open();

        // 3. Verifica se o banco já existe no PostgreSQL
        var checkCmd = dbConnection.CreateCommand();
        checkCmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
        var exists = checkCmd.ExecuteScalar() != null;

        // 4. Se não existir, executa o CREATE DATABASE
        if (!exists)
        {
            var createCmd = dbConnection.CreateCommand();
            createCmd.CommandText = $"CREATE DATABASE \"{databaseName}\"";
            createCmd.ExecuteNonQuery();
        }
    }
    
    private static void MigrationDataBase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}