using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorAdSample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BlazorAdSample.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorAdSample.ServerAPI"));

            builder.Configuration.AddConfiguration(
                new ConfigurationBuilder().AddEnvironmentVariables().Build());
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2CClient", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://georgiadb2c.onmicrosoft.com/94e54391-c708-4077-af9a-08acda464536/API.Access");
            });

            await builder.Build().RunAsync();
        }
    }
}
