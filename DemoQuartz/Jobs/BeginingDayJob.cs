using DemoQuartz.Settings;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoQuartz.Jobs
{
    public class BeginingDayJob : IJob
    {
        private readonly ILogger<BeginingDayJob> _logger;
        public BeginingDayJob(ILogger<BeginingDayJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                InstanceSettings.IsBeginingDay = true;
                await Task.Delay(1000);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
