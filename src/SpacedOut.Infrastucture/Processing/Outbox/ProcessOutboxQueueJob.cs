using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpacedOut.Infrastucture.Data;
using SpacedOut.SharedKernel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.Processing.Outbox
{
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
    internal class ProcessOutboxQueueJob : IHostedService, IDisposable
    {
        private int _executionCount = 0;
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;

        public ProcessOutboxQueueJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(
                DoWork,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5)
            );

            await Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            // https://stackoverflow.com/a/53809870/234132
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = GetDbContext(scope);
                if (dbContext != null)
                {
                    OutboxMessage? nextInQueue;
                    while ((nextInQueue = GetNextInQueue(dbContext)) != null)
                    {
                        try
                        {
                            var handler = GetHandler(scope, nextInQueue.Key);

                            if (handler == null) throw new InvalidOperationException($"'{nextInQueue.Key}' outbox hander not found");

                            handler.Process(nextInQueue.Data);

                            nextInQueue.MarkProcessed();
                        }
                        catch
                        {
                            nextInQueue.MarkFailed();
                        }

                        dbContext.SaveChanges();
                    }
                }
            }

            var count = Interlocked.Increment(ref _executionCount);

            Debug.WriteLine($"{GetType().Name} count : {count}");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private static AppDbContext? GetDbContext(IServiceScope scope)
        {
            return scope
                .ServiceProvider
                .GetService<AppDbContext>();
        }

        private static OutboxMessage? GetNextInQueue(AppDbContext dbContext)
        {
            var cutoff = SystemTime.UtcNow();

            return dbContext
                .OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .Where(x => x.ProcessOnUtc <= cutoff)
                .OrderBy(x => x.ProcessOnUtc)
                .FirstOrDefault();
        }

        private static BaseOutboxMessageHandler? GetHandler(IServiceScope scope, string key)
        {
            return GetEnumerableOfType<BaseOutboxMessageHandler>(scope).FirstOrDefault(h => h.Key == key);
        }

        // https://stackoverflow.com/a/6944605/234132
        private static IEnumerable<T> GetEnumerableOfType<T>(IServiceScope scope) where T : class
        {
            var baseType = typeof(T);
            var types = Assembly
                .GetAssembly(baseType)
                ?.GetTypes()
                ?.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));

            if (types != null)
            {
                foreach (var type in types)
                {
                    var instance = (T?)ActivatorUtilities.CreateInstance(scope.ServiceProvider, type); // https://stackoverflow.com/a/52645270/234132
                    if (instance != null)
                    {
                        yield return instance;
                    }
                }
            }
        }
    }
}