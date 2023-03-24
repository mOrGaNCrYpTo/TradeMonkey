using Microsoft.Extensions.DependencyInjection;

namespace TradeMonkey.Core.DI
{
    public static class AssemblyScanner
    {
        public static IServiceCollection AddTradeMonkeyServices(this IServiceCollection services)
        {
            var baseNamespace = "TradeMonkey";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Filter the assemblies you want to scan based on the base namespace
            var filteredAssemblies = assemblies
                .Where(predicate: assembly =>
                    assembly.FullName.StartsWith(baseNamespace, StringComparison.OrdinalIgnoreCase));

            // Scan the types in the filtered assemblies
            foreach (var assembly in filteredAssemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => type.Namespace != null && type.Namespace.StartsWith(baseNamespace, StringComparison.OrdinalIgnoreCase));

                foreach (var type in types)
                {
                    // Check if the type is a class and not abstract
                    if (type.IsClass && !type.IsAbstract)
                    {
                        // Assuming you have a common interface convention like I[ClassName]
                        var interfaceType = type.GetInterface($"I{type.Name}");

                        // Register the type with the DI container
                        if (interfaceType != null)
                        {
                            services.AddTransient(interfaceType, type);
                        }
                    }
                }
            }

            return services;
        }
    }
}