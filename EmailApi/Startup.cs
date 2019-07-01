using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EmailApi.Domain.Models;
using EmailApi.Domain.Repositories;
using EmailApi.Domain.Services;
using EmailApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using EmailApi.Persistence.Contexts;
using EmailApi.Persistence.Repositories;
using EmailApi.Filter;
using MassTransit;
using EmailApi.Consumers;

namespace EmailApi
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
            services.AddLogging(x => x.AddConsole());
            services.AddCors();
            services.AddMvc(x => x.Filters.Add<GeneralExceptionFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDBContext>(x => x.UseInMemoryDatabase("Test"));
            //above I configure the db context. 
            //use inmemeory provider to not connect a real db to test app
            services.AddScoped<IMailRepository, MailRepository>();
            services.AddScoped<IMailService, MailService>();

            //used scoped lifetime because these calasses internally have to use
            //the db context class.it specifies the same scope.
            services.AddMassTransit(serviceCfg =>
            {
                serviceCfg.AddConsumers(typeof(Startup).Assembly);
                serviceCfg.AddBus(serviceProvider =>
                {

                    IBusControl bus = Bus.Factory.CreateUsingInMemory((cfg) =>
                    {
                        cfg.ReceiveEndpoint("mail_scheduled_queue", e =>
                        {
                            e.ConfigureConsumer<SendMailService>(serviceProvider);
                        });
                    });
                    return bus;
                });
            });


            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "Email Api", Version = "v1" }));
            services.ConfigureSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IServiceScopeFactory serviceProvider)
        {
            applicationLifetime.ApplicationStarted.Register(() => StartBus(serviceProvider));
            applicationLifetime.ApplicationStopping.Register(() => StopBus(serviceProvider));

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Email Api v1");
            });


        }

        private void StartBus(IServiceScopeFactory serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var busControl = scope.ServiceProvider.GetRequiredService<IBusControl>();
                busControl.Start();
            }
        }

        private void StopBus(IServiceScopeFactory serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var busControl = scope.ServiceProvider.GetRequiredService<IBusControl>();
                busControl.Stop();
            }
        }
    }
}
