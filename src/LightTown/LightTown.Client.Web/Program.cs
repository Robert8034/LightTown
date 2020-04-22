using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using LightTown.Client.Services.Authentication;
using LightTown.Client.Services.Projects;
using LightTown.Client.Services.Users;
using LightTown.Client.Services.Validation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Client.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY")))
            {
                WebAssemblyHttpMessageHandlerOptions.DefaultCredentials = FetchCredentialsOption.Include;
            }

            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<IValidationService, ValidationService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IProjectService, ProjectService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<UserSessionService>();
            builder.Services.AddBaseAddressHttpClient();

            var host = builder.Build();

            var userSessionService = host.Services.GetRequiredService<UserSessionService>();
            if (await userSessionService.TryLoadLocalUser())
            {
                await userSessionService.LoadUser();
            }

            await host.RunAsync();
        }
    }
}
