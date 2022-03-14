using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace AuthorizationServer
{
    public class SeedAuthServerData
    {
        private readonly IServiceProvider _serviceProvider;
      

        public SeedAuthServerData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
       
        }

        public async ValueTask Seed(CancellationToken cancellationToken=default)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    }
                }, cancellationToken);
            }
        }

    }
}
