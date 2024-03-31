using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kafka_consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var counter = 1;
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            
            Console.WriteLine($"Hit counter - {counter++}");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
