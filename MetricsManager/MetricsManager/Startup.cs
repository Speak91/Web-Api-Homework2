using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Models;
using MetricsManager.Models.Request;
using MetricsManager.Services;
using MetricsManager.Services.Impl;
using MetricsManager.Services.Interfaces;
using MetricsManager.Services.Repositories;
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
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
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
            services.AddHttpClient();

            services.AddControllers();

            #region CPU
            services.AddHttpClient<ICpuMetricsAgentClient,
                CpuMetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                }));
            #endregion

            #region DOTNET
            services.AddHttpClient<IDotNetMetricsAgentClient,
                DotNetMetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                }));
            #endregion

            #region RAM
            services.AddHttpClient<IRamMetricsAgentClient,
                RamMetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                }));
            #endregion

            #region HDD
            services.AddHttpClient<IHddMetricsAgentClient,
                HddMetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                }));
            #endregion

            #region Network
            services.AddHttpClient<INetworkMetricsAgentClient,
                NetworkMetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                }));
            #endregion

            services.AddSingleton<IAgentsRepository, AgentsRepository>();
            
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавл¤ем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключени¤
                    .WithGlobalConnectionString(SQLConnectionString.ConnectionString)
                    // подсказываем где искать классы с миграци¤ми
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers().
                  AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsManager", Version = "v1" });
                
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsManager v1"));
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
