using NotifyWorker;
using NotifyWorker.Gateway;
using NotifyWorker.Queue;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<MailtrapSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailerGateway, MailtrapAdapter>();
builder.Services.AddSingleton<IOrderGateway, OrderGatewayHttp>();
builder.Services.AddSingleton<IQueue, RabbitMQAdapter>();
builder.Services.AddHostedService<QueueController>();




var host = builder.Build();
host.Run();
