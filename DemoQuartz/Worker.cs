using DemoQuartz.DB;
using DemoQuartz.Ip;
using DemoQuartz.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoQuartz
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DbOptions _db;
        private readonly IPInfo _ipInfo;
        private readonly IDatabase _database;
        private readonly Configs _config;
        private List<string> ListXN = new List<string>();

        /// <summary>
        /// Contructor Method
        /// </summary>
        public Worker(ILogger<Worker> logger, DbOptions db, IPInfo ipInfo, IDatabase database, Configs config)
        {
            _logger = logger;
            _db = db;
            _ipInfo = ipInfo;
            _database = database;
            _config = config;
        }
        /// <summary>
        /// Start app
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync runing ...");
            if (!InstanceSettings.IsBeginingDay || !XnCodeSettings.IsChangeXnCode) await LoadConfig();

            try
            {
                ListXN = _database.LayDanhSachXn(ListXN, _config.Year, _config.XnCodeMin, _config.XnCodeMax, _config.SystemWeb);
            }
            catch (Exception)
            {

                throw;
            }

            await base.StartAsync(cancellationToken);
        }
        /// <summary>
        /// Stop app
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync runing ...");
            await base.StopAsync(cancellationToken);
        }
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunAgain();
                await PutFileAsync();
                await Task.Delay(2000);
            }
            await Task.Delay(1000, stoppingToken);
        }

        private async Task RunAgain()
        {
            try
            {
                if (InstanceSettings.IsBeginingDay || XnCodeSettings.IsChangeXnCode)
                {
                    _logger.LogInformation("RunAgain runing ...");
                    await LoadConfig();
                }
                // Set lại trạng thái
                InstanceSettings.IsBeginingDay = false;
                XnCodeSettings.IsChangeXnCode = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task LoadConfig()
        {
            _logger.LogInformation("LoadConfig runing ...");
            await Task.Delay(1000);
        }

        private async Task PutFileAsync()
        {
            _logger.LogInformation("PutFileAsync runing ...");
            await Task.Delay(1000);
        }
    }
}
