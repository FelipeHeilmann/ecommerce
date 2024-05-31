using Microsoft.Extensions.Configuration;
using ProjectionWorker.Context;
using ProjectionWorker.Gateway;
using ProjectionWorker.OrderCanceled;
using ProjectionWorker.OrderCheckedout;
using ProjectionWorker.Queue;
using ProjectionWorker.TransactionStatusChanged;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IQueue, RabbitMQAdapter>(provider =>
{
    var rabbitMQAdapter = new RabbitMQAdapter(builder.Configuration);
    rabbitMQAdapter.Connect();
    return rabbitMQAdapter;
});
builder.Services.AddHostedService<OrderCheckedoutConsumer>();
builder.Services.AddHostedService<OrderCanceledConsumer>();
builder.Services.AddHostedService<TransactionStatusChangedConsumer>();
builder.Services.AddSingleton<OrderContext>();

builder.Services.AddSingleton<IOrderGeteway, OrderGatewayHttp>();

var host = builder.Build();
host.Run();
