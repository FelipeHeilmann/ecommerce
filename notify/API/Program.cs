using API;
using API.Consumers;
using API.Gateway;
using API.Queue;
using Application.Abstractions.Queue;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
