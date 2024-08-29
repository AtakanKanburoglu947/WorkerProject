using Microsoft.EntityFrameworkCore;
using WorkerService;
using WorkerService.Context;
using WorkerService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(DtoMapper).Assembly);
builder.Services.AddDbContext<dbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkerContext")),ServiceLifetime.Scoped);
builder.Services.AddDbContext<MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCContext")), ServiceLifetime.Scoped);
builder.Services.AddScoped<APIService>();
builder.Services.AddScoped<dbService>();
builder.Services.AddScoped<mvcService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
