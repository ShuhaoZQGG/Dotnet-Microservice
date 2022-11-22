using CommandService.Configurations;
using CommandService.Data;
using CommandService.Event;
using CommandService.Message;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMq>();
builder.Services.Configure<RabbitMq>(builder.Configuration.GetSection("RabbitMq"));
Console.WriteLine(rabbitMqConfig.Host);
Console.WriteLine(rabbitMqConfig.Port);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddLogging(opt =>
{
  opt.AddSimpleConsole(opt =>
  {
    opt.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
  });
});
builder.Services.AddDbContext<AppDbContext>(opt => {
  ILogger logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
  opt.UseInMemoryDatabase("InMem").LogTo((message) => logger.LogInformation("SQL {sqlTrace}", message), LogLevel.Information);
 
});
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEvent, Event>();
builder.Services.AddHostedService<MessageBusSubscriber>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
