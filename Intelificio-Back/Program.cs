using Backend.Common.Behavior;
using Backend.Common.Helpers;
using Backend.Common.Profiles;
using Backend.Common.Security;
using Backend.Features.Community.Commands.Create;
using Backend.Models;
using Backend.Models.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddSingleton<SendMail>();


builder.Services.AddDbContext<IntelificioDbContext>(
    options =>
    {
        _ = options
            .UseMySQL(builder.Configuration.GetConnectionString("Default") ?? "")
            .AddInterceptors(new SoftDeleteInterceptor());
    });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
    cfg.AddProfile<CommunityProfile>();
    cfg.AddProfile<BuildingProfile>();
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<LocationProfile>();
    cfg.AddProfile<UnitProfile>();
    cfg.AddProfile<ContactProfile>();
    cfg.AddProfile<CommonSpaceProfile>();
    cfg.AddProfile<ReservationProfile>();
    cfg.AddProfile<AttendeeProfile>();
    cfg.AddProfile<MaintenanceProfile>();
    cfg.AddProfile<ConfirmEmailProfile>();
});

builder.Services.AddIdentity<User, Role>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
        cfg.Password.RequiredLength = 8;
        cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        cfg.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<IntelificioDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Secret"))),
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddValidatorsFromAssembly(typeof(CreateCommunityCommandValidator).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("All", cfg =>
    {
        _ = cfg
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("All", cfg =>
    {
        _ = cfg
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("All");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.UseMigrations();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();