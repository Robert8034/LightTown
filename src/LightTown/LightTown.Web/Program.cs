using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using LightTown.Web.Services.Authentication;
using LightTown.Web.Services.Users;
using LightTown.Web.Services.Validation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LightTown.Web
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
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<UserSessionService>();
            builder.Services.AddBaseAddressHttpClient();

            await builder.Build().RunAsync();
        }
    }
}
