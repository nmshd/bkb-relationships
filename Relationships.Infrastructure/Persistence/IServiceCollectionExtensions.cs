using Microsoft.Extensions.DependencyInjection;
using Relationships.Application.Infrastructure;
using Relationships.Infrastructure.Persistence.ContentStore;
using Relationships.Infrastructure.Persistence.Database;

namespace Relationships.Infrastructure.Persistence;

public static class IServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, Action<PersistenceOptions> setupOptions)
    {
        var options = new PersistenceOptions();
        setupOptions?.Invoke(options);

        services.AddPersistence(options);
    }

    public static void AddPersistence(this IServiceCollection services, PersistenceOptions options)
    {
        services.AddDatabase(options.DbOptions);

        services.AddAzureStorageAccount(options.BlobStorageOptions);
        services.AddScoped<IContentStore, BlobStorageContentStore>();
    }
}

public class PersistenceOptions
{
    public Database.IServiceCollectionExtensions.DbOptions DbOptions { get; set; } = new();
    public AzureStorageAccountOptions BlobStorageOptions { get; set; } = new();
}
