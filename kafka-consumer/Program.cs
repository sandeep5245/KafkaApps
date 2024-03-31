using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using kafka_consumer;
using InventoryConsumer.Services;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ConsumerService>();

var host = builder.Build();
host.Run();
