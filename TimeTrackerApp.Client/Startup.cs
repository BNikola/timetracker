using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using TimeTrackerApp.Client.Security;
using TimeTrackerApp.Client.Services;

namespace TimeTrackerApp.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();        // svi servisi za autorizaciju
            services.AddTokenAuthenticationStateProvider();
            services.AddTransient<ApiService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
