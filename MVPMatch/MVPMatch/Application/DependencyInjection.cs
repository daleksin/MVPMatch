using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MVPMatch.Application.Services;

namespace MVPMatch.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IEntityService, EntityService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IActionService, ActionService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
