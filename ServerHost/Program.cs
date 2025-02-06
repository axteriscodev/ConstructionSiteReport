using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerHost.Services;
using ServerHost.Services.Interfaces;
using System.Text;
using TDatabase.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// servizio di autenticazione con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = ConfigurationService.GetConfiguration("Jwt:Issuer"),
            ValidateAudience = true,
            ValidAudience = ConfigurationService.GetConfiguration("Jwt:Audience"),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                ConfigurationService.GetConfiguration("Jwt:SecurityKey")!)),
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<IMailService, MailKitService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
//}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
