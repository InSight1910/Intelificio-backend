using AZ_204.Common.Profilers;
using Intelificio_Back.Common.Profiles;
using Intelificio_Back.Common.Security;
using Intelificio_Back.Models;
using Intelificio_Back.Models.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<TokenProvider>();

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
    cfg.AddProfile<UserProfile>();
});

builder.Services.AddIdentity<User, Role>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<IntelificioDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Secret"))),
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddMediatR(cfg =>
{
    _ = cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});


var app = builder.Build();

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

app.Run();
