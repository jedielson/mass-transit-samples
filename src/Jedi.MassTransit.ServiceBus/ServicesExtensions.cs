using Jedi.MassTransit.ServiceBus.Consumers;
using Jedi.MassTransit.ServiceBus.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi.MassTransit.ServiceBus
{
    public static class ServicesExtensions
    {
        public static void AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IniciarExecucao>();

            services.AddMassTransit(x =>
            {
                // add the consumer to the container
                x.AddConsumer<IniciarExecucaoConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {
                
                var host = cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.SetLoggerFactory(provider.GetService<ILoggerFactory>());

                cfg.Publish<IniciarExecucao>(t =>
                {
                    t.BindQueue("web-service-endpoint", "web-service-endpoint");
                    t.Durable = true;
                    t.AutoDelete = false;
                });

                cfg.ReceiveEndpoint("web-service-endpoint",  e =>
                {
                    e.Durable = true;
                    e.AutoDelete = false;
                    e.AutoStart = true;
                    e.PrefetchCount = 1;

                    //e.UseMessageRetry(x => x.Interval(2, 100));                   

                    e.Consumer<IniciarExecucaoConsumer>(provider);
                    EndpointConvention.Map<IniciarExecucao>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<IniciarExecucao>());

            services.AddSingleton<IHostedService, BusService>();
        }
    }
}
