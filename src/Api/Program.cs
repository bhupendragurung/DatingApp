

using Api;
using FluentValidation;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());
builder.Services.AddControllers();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")??throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDataProtection();
builder.Services
    .AddIdentityCore<AppUser>()
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
 builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
