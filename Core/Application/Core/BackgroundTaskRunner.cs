using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Infrastructure
{
    public class BackgroundTaskScheduler : IBackgourndTaskScheduler
    {
        private BlockingCollection<BackgroundTask> tasks = new BlockingCollection<BackgroundTask>();
        public void EnqueTask(BackgroundTask task)
        {
            tasks.Add(task);
        }
        public BackgroundTask Take()
        {
            return tasks.Take();
        }
    }
    public class BackgroundTaskRunner : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly IBackgourndTaskScheduler taskScheduler;
        private readonly ILogger<BackgroundTaskRunner> logger;        

        public BackgroundTaskRunner(IServiceProvider services, IBackgourndTaskScheduler taskScheduler, ILogger<BackgroundTaskRunner> logger)
        {
            this.services = services;
            this.taskScheduler = taskScheduler;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ThreadPool.QueueUserWorkItem(runForEver, stoppingToken);
            
            //return Task.CompletedTask;
        }
        void runForEver(object state)
        {
            var stoppingToken = (CancellationToken)state;
            while (true)
            {
                var task = taskScheduler.Take();
                if (task == null || task.Task == null)
                    continue;
                if (stoppingToken.IsCancellationRequested)
                    break;
                ThreadPool.QueueUserWorkItem(runTask, task);
            }
        }

        private void runTask(object state)
        {
            try
            {
                var task = (BackgroundTask)state;
                using (var scope = services.CreateScope())
                {
                    task.Task(scope).Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing BackgroundTask");
            }            
        }

        
    }

    
}
