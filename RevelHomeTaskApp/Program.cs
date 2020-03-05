using Microsoft.Extensions.DependencyInjection;
using RevelHomeTaskApp.Service;
using System;

namespace RevelHomeTaskApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Enter a wep page URL (f.e. http://www.google.com) :");

            string url = Console.ReadLine();

            serviceProvider.GetService<TagInsightApp>().Run(url);
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ITagInsightsService, TagInsightsService>();
            services.AddSingleton<TagInsightApp>();

            return services;
        }
    }
}
