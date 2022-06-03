using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.Jobs;
using MetricsAgent.Services;
using MetricsAgent.Services.Interfaces;
using MetricsAgent.Services.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MetricsAgent
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
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                rb.AddSQLite().WithGlobalConnectionString(Configuration.GetSection("Settings:DatabaseOptions:ConnectionString").Value)
                .ScanIn(typeof(Startup).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());


            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton<DotnetMetricJob>();

            services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * ? * * *"));
            services.AddSingleton(new JobSchedule(typeof(RamMetricJob), "0/5 * * ? * * *"));
            services.AddSingleton(new JobSchedule(typeof(HddMetricJob), "0/5 * * ? * * *"));
            services.AddSingleton(new JobSchedule(typeof(NetworkMetricJob), "0/5 * * ? * * *"));
            services.AddSingleton(new JobSchedule(typeof(DotnetMetricJob), "0/5 * * ? * * *"));
            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "MetricsAgent", 
                    Version = "v1",
                Description = "Здесь можно посмотреть Api нашего сервиса",
                    TermsOfService = new Uri("https://example.com"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Illarionov V.",
                        Email = "mail@mail.ru",
                        Url = new Uri("https://google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Указать под какой лицензией все опубликовано",
                        Url = new Uri("http://example.com")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            migrationRunner.MigrateUp();
        }
    }
}
