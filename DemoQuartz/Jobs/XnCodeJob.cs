using DemoQuartz.DB;
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
    [DisallowConcurrentExecution]
    public class XnCodeJob : IJob
    {
        private readonly ILogger<XnCodeJob> _logger;
        private readonly IDatabase _database;
        private readonly Configs _config;
        public XnCodeJob(ILogger<XnCodeJob> logger, IDatabase database, Configs configs)
        {
            _logger = logger;
            _database = database;
            _config = configs;
        }

        public async Task Execute(IJobExecutionContext context)
       {
            try
            {
                _logger.LogInformation("XnCodeJob running ...");

                List<string> lstXnCode = new List<string>();
                lstXnCode = _database.LayDanhSachXn(XnCodeSettings.ListXnCode, _config.Year, _config.XnCodeMin, _config.XnCodeMax, _config.SystemWeb);
                if (lstXnCode.Count > 0)
                {
                    XnCodeSettings.ListXnCode = lstXnCode;
                    XnCodeSettings.IsChangeXnCode = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            await Task.Delay(1000);
        }
    }
}
