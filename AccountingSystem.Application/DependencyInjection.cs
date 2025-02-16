using AccountingSystem.Application.Services.Auth;
using AccountingSystem.Application.Services.Tenant;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITenantService, TenantService>();
            return services;
        }
    }
}