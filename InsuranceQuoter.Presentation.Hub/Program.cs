﻿namespace InsuranceQuoter.Presentation.Hub
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using InsuranceQuoter.Acl.Insurer.Service.Settings;
    using InsuranceQuoter.Infrastructure.Constants;
    using InsuranceQuoter.Infrastructure.Extensions;
    using InsuranceQuoter.Infrastructure.Message.Requests;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    [ExcludeFromCodeCoverage]
    internal class Program
    {
        public static void Main(string[] args)
        {
            var applicationSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<ApplicationSettings>();

            BuildWebHost(args, applicationSettings).Build().Run();
        }

        public static IHostBuilder BuildWebHost(string[] args, ApplicationSettings applicationSettings) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    c =>
                        c.UseStartup<Startup>()
                            .UseUrls("https://localhost:9001")
                )
                .UseNServiceBus(
                    _ =>
                    {
                        var endpointConfiguration = new EndpointConfiguration(MessagingEndpointConstants.PresentationHub);

                        endpointConfiguration.SendFailedMessagesTo(MessagingEndpointConstants.PresentationHub + ".Error");

                        endpointConfiguration.EnableInstallers();
                        TransportExtensions<AzureServiceBusTransport> transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                        transport.ConnectionString(applicationSettings.ServiceBusEndpoint);

                        transport.SubscriptionNamingConvention(s => s.Replace("Infrastructure.", string.Empty));
                        transport.SubscriptionRuleNamingConvention(t => t.FullName.Replace("Infrastructure.", string.Empty));

                        endpointConfiguration.AddUnobtrusiveMessaging();

                        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);
                        endpointConfiguration.TimeoutManager().LimitMessageProcessingConcurrencyTo(10);

                        transport.Routing().RouteToEndpoint(
                            typeof(QuoteRequest),
                            MessagingEndpointConstants.SagaService
                        );

                        return endpointConfiguration;
                    });
    }
}