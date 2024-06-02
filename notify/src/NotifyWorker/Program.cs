using NotifyWorker;
using NotifyWorker.Consumers;
using NotifyWorker.Gateway;
using NotifyWorker.Queue;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<MailtrapSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailerGateway, MailtrapAdapter>();
builder.Services.AddSingleton<IOrderGateway, OrderGatewayHttp>();
builder.Services.AddSingleton<IQueue, RabbitMQAdapter>(provider =>
{
    var rabbitMQAdapter = new RabbitMQAdapter(builder.Configuration);
    rabbitMQAdapter.Connect();
    return rabbitMQAdapter;
});
builder.Services.AddHostedService<CustomerCreatedConsumer>();
builder.Services.AddHostedService<OrderCheckedoutConsumer>();
builder.Services.AddHostedService<OrderCanceledConsumer>();
builder.Services.AddHostedService<OrderPaymentStatusChangedConsumer>();



var host = builder.Build();
host.Run();
