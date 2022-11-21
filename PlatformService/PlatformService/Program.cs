using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using PlatformService.Configuration;
using PlatformService.Data;
using PlatformService.MessageBroker;
using PlatformService.SyncMessageServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Comment to turn off windows user authentication
//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

// Add DbContext to services
var sqlServerConfig = builder.Configuration.GetSection("SqlServer").Get<SqlServer>();
var rabbitmqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMq>();
if (builder.Environment.IsProduction())
{
  Console.WriteLine("---> Production Environment, Using Sql Server Database");
  builder.Services.AddDbContext<AppDbContext>(opt =>
  {
    ILogger logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
    opt.UseSqlServer(sqlServerConfig.PlatformsConnectionString).LogTo((message) => logger.LogInformation("SQL {sqlTrace}", message), LogLevel.Information);
  });
}
else
{
  Console.WriteLine("---> Development Environment, Using In Memory Database");
  builder.Services.AddDbContext<AppDbContext>(opt =>
  {
    opt.UseInMemoryDatabase("InMem");
  });
}


// Register services for dependency injection
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register Configurations
builder.Services.Configure<CommandService>(builder.Configuration.GetSection("CommandService"));

builder.Services.AddHttpClient<IMessageHttpClient, MessageHttpClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddLogging(opt =>
{
  opt.AddSimpleConsole(opt =>
  {
    opt.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
  });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
// Comment to run docker container
//app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
