using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ConversionHive.Data;
using ConversionHive.Repository;
using ConversionHive.Repository.Implementations;
using ConversionHive.Services;
using ConversionHive.Services.Implementations;
using System.Text;
using System.Threading.RateLimiting;
using ConversionHive;
using Decoder = ConversionHive.Services.Implementations.Decoder;
using Encoder = ConversionHive.Services.Implementations.Encoder;

const string corsPolicy = "allow all origins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please provide a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = EnvironmentConfigHelper.GetIssuer(builder.Configuration, builder.Environment),
            ValidAudience = EnvironmentConfigHelper.GetAudience(builder.Configuration, builder.Environment),
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(EnvironmentConfigHelper.GetKey(builder.Configuration, builder.Environment)))
        };
    });
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IMailConfigService, MailConfigService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IJwtProcessor, JwtProcessor>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IEncoder, Encoder>(_ =>
    new Encoder(EnvironmentConfigHelper.GetSalt(builder.Configuration, builder.Environment)));
builder.Services.AddTransient<IDecoder, Decoder>(_ =>
    new Decoder(EnvironmentConfigHelper.GetSalt(builder.Configuration, builder.Environment)));
builder.Services.AddDbContext<SendMailDbContext>(options =>
    options.UseNpgsql(EnvironmentConfigHelper.GetConnectionString(builder.Configuration, builder.Environment))
);
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("fixed-by-ip", httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            });
    });
});


var app = builder.Build();

var context = app.Services.CreateScope()
    .ServiceProvider.GetRequiredService<SendMailDbContext>();

if (app.Environment.IsDevelopment())
{
    await context.Database.EnsureDeletedAsync();
}

await context.Database.EnsureCreatedAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRateLimiter();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(corsPolicy);

app.UseAuthorization();

app.MapControllers();


app.Run();