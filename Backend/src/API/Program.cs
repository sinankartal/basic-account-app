using System.Text;
using API.Middlewares;
using Application;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Infrastructure.Azure;
using Infrastructure.Azure.AzureServiceBus.Sender;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Data;
using Persistence.IRepositories;
using Persistence.Models;
using Serilog;
using Serilog.AspNetCore;
using TransactionService.MessageQueue.AzureServiceBus.TransactionResolver;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Account App API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

//enable CORS
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//logger
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("../logs/api-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(typeof(ILogger), Log.Logger);

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey"))),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

//AutoMapper
var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped(typeof(IAccountRepository), typeof(AccountRepository));
builder.Services.AddScoped(typeof(IAccountService), typeof(AccountService));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IServiceBus, ServiceBus>();

builder.Services.AddScoped<IServiceBusResolver, ServiceBusResolver>();

builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("db")));

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });


app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

AddSeedData(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("corsapp");
app.Run();

static void AddSeedData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var accountDbContext = scope.ServiceProvider.GetService<AccountDbContext>();
    int i = 0;
    accountDbContext.Users.AddRange(Enumerable.Range(1, 10).Select(x => new User
    {
        Id = Guid.NewGuid(),
        Name = $"Sinan {x}",
        Surname = $"Kartal {x}"
    }));

    accountDbContext.SaveChanges();
}