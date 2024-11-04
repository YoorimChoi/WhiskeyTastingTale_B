using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Whiskey_TastingTale_Backend.Data.Repository;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Middleware;
using Serilog;
using Whiskey_TastingTale_Backend.Services;
using Elastic.Apm.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


//Use Serilog 
Serilog.Log.Logger = new LoggerConfiguration()
     //.Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.MessageTemplate.Text.Contains("[API]"))  // 조건: "Database" 포함
        .WriteTo.File("logs/API-.log", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.MessageTemplate.Text.Contains("[ERR]"))  // 조건: "Database" 포함
        .WriteTo.File("logs/Error-.log", rollingInterval: RollingInterval.Day))
    .CreateLogger();
builder.Services.AddSerilog();


//Add Repository Service 
builder.Services.AddTransient<WhiskeyRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<ReviewRepository>();
builder.Services.AddTransient<WishRepository>();
builder.Services.AddTransient<WhiskeyRequestRepository>();
builder.Services.AddTransient<NotificationRepository>();


builder.Services.AddSingleton<NotificationService>();

builder.Services.AddAllElasticApm();

//Add Db Context
builder.Services.AddDbContext<WhiskeyContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));
builder.Services.AddDbContext<UserContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));
builder.Services.AddDbContext<ReviewContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));
builder.Services.AddDbContext<WishContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));
builder.Services.AddDbContext<WhiskeyRequestContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));
builder.Services.AddDbContext<NotificationContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("localmssql")));

//Setting JWT 
builder.Services.AddAuthentication(options => //Using jwt as the basic authentication method
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSignalR();

var app = builder.Build();


app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseElasticApm();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");



app.Run();
