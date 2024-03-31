using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;

namespace InventoryConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;

        private readonly ILogger<ConsumerService> _logger;

        private readonly string topic;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {
            _logger = logger;
            topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? "InventoryUpdates";

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }

            _consumer.Close();
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;

                _logger.LogInformation($"Received inventory update: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
}