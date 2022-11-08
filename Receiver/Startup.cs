using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Receiver", Version = "v1" });
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RabbitMqSender>();
                x.AddConsumer<RabbitMqPublisher>();
                x.AddConsumer<RabbitMqRequestResponse>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                      {
                          h.Username("guest");
                          h.Password("guest");
                      });

                    cfg.ReceiveEndpoint("send-tutorial", x =>
                    {
                        x.Consumer<RabbitMqSender>(context);
                    });

                    cfg.ReceiveEndpoint("publish-tutorial", x =>
                    {
                        x.Consumer<RabbitMqPublisher>(context);
                    });

                    cfg.ReceiveEndpoint("request-response-tutorial", x =>
                    {
                        x.Consumer<RabbitMqRequestResponse>(context);
                    });
                });

                services.AddMassTransitHostedService();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Receiver v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
