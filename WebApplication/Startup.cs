using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using WebApplication1.CustomPolicy;
using WebApplication1.CustomPolicy.Extensions;

namespace WebApplication1
{
    public class Startup
    {
        public ServiceProvider ServiceProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var policyFactory = new CustomRecoveryPolicyFactory();
            services.AddSingleton<ICustomRecoveryPolicyFactory>(policyFactory);

            var endpointConfiguration = new EndpointConfiguration("HelloApplication");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("HelloApplication.Error");
            endpointConfiguration.EnableInstallers();

            
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(im =>
            {
                im.NumberOfRetries(2);
            });
            recoverability.Delayed(del =>
            {
                del.NumberOfRetries(3).TimeIncrease(TimeSpan.FromSeconds(5));
            });
            //here is the magic. Sürmüyorsun bile :) 
            recoverability.CustomPolicy(policyFactory.GetRecoverabilityAction());

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost;username=guest;password=guest;RequestedHeartbeat=600");
            transport.UseConventionalRoutingTopology();
            transport.DelayedDelivery().EnableTimeoutManager();

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            this.ServiceProvider = services.BuildServiceProvider();

            services.AddSingleton<IEndpointInstance>(endpoint);
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
