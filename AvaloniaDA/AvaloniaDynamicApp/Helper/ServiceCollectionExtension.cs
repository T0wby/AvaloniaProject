using AvaloniaDynamicApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaDynamicApp.Helper
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddTransient<MainWindowViewModel>();
        }
    }
}
