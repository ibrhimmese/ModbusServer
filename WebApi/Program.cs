using Application;
using Application.ExceptionTypes;
using Domain.BaseProjeEntities.IdentityEntities;
using Infrastructure;
using Infrastructure.Exceptions.Extensions;
using Infrastructure.StorageConcretes.Local;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Contexts;
using ServerAPI.AuthorizationFiters;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddScoped<AuthenticationFilter>();
builder.Services.AddScoped<RolePermissionFilter>();


builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

}).AddEntityFrameworkStores<BaseDbContext>()
          .AddDefaultTokenProviders();

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices();

builder.Services.AddStorage<LocalStorage>();

builder.Services.AddDistributedMemoryCache();
//builder.Services.AddStackExchangeRedisCache(opt=>opt.Configuration="localhost:6379");

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => //Cors Settings
{
    options.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials().
    WithOrigins("http://192.168.1.11:5173", "https://192.168.1.11:5173", "http://192.168.1.11:5297", "https://192.168.1.11:6098")

    //.SetIsOriginAllowed(policy => true)
    );
}); //Addedd

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin", options =>
{
    var securityKey = builder.Configuration["Token:SecurityKey"];
    if (string.IsNullOrEmpty(securityKey))
    {
        throw new OperationException("Security key is not configured.");
    }

    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,            //Olu?turulacak tokenin kimlerin hangi sitelerin kullanaca??n? belirtece?iz.
        ValidateIssuer = true,              //Olu?turulacak token de?erini kimin da??tt???n? ifade edecez.
        ValidateLifetime = true,            //Olu?turulan token de?erinin s?resini kontrol edecek olan do?rulamad?r.
        ValidateIssuerSigningKey = true,    //?retilecek token de?erinin uygulamam?za ait bir de?er oldu?unu ifade eden secury key verisini do?rulamas?d?r.

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

        NameClaimType = ClaimTypes.Name
    };
});


builder.Services.AddSwaggerGen(setup => //Addedd
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
}); //Jwt



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.ConfigureCustomExceptionMiddleware(); //Middleware
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
