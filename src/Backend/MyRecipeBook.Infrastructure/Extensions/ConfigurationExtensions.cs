using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    // Um único método que vai direto ao ponto: busca a string do Postgres.
    public static string ConnectionString(this IConfiguration configuration)
    {
            // Certifique-se de que o nome aqui ("ConnectionPostgresSQL") 
            // é EXATAMENTE o mesmo que está no seu appsettings.json
             return configuration.GetConnectionString("ConnectionPostgresSQL")!;
    }
}