﻿namespace InsuranceQuoter.Presentation.Hub
{
    using InsuranceQuoter.Presentation.Hub.Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors("Open");

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapHub<QuoteHub>("/quotehub");
                });
        }
    }
}