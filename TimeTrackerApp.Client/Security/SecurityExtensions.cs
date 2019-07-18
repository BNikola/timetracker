using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Client.Security
{
    public static class SecurityExtensions
    {
        public static void AddTokenAuthenticationStateProvider(this IServiceCollection services)
        {
            services.AddScoped<TokenAuthenticationStateProvider>(); // vise tipova life-time-a
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());   // primjer Steve Sanderson-a

        }
    }
}
