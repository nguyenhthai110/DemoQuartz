using DemoQuartz.Ip;
using DemoQuartz.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;

namespace DemoQuartz
{
    public static class ServiceCollectionExtensions
    {
        private const string QuartzSectionName = "Quartz";
        private const string ListIPS = "ListIPS";

        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();
            var quartzOptions = new QuartzOptions();
            config.Bind(QuartzSectionName, quartzOptions);
            services.AddSingleton(quartzOptions);
            services.Configure<QuartzOptions>(config.GetSection(QuartzSectionName));
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            if (quartzOptions != null && quartzOptions.Enabled)
            {
                foreach (var item in quartzOptions.JobOption)
                {
                    if (item.Enabled)
                    {
                        var type = Type.GetType($"{item.JobType}");

                        services.AddSingleton(type);

                        services.AddSingleton(new JobSchedule(new System.Guid(),
                            jobType: type,
                            jobName: item.JobName,
                            cronExpression: item.CronExpression));
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddIP(this IServiceCollection services)
        {
            try
            {
                var svcProvider = services.BuildServiceProvider();
                var config = svcProvider.GetRequiredService<IConfiguration>();
                var ipInfo = new IPInfo();
                ipInfo = config.GetSection(ListIPS).Get<IPInfo>();
                if (ipInfo.IPS.Count > 0)
                {
                    services.AddSingleton(ipInfo);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return services;
        }
    }
}
