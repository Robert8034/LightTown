using System.Threading.Tasks;
using LightTown.Web.Services.Validation;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<IValidationService, ValidationService>();

            await builder.Build().RunAsync();
        }
    }
}
