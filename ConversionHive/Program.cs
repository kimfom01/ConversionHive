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
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") ??
                          throw new Exception("Jwt issuer key not found"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience") ??
                            throw new Exception("Jwt audience key not found"),
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key") ??
                               throw new Exception("Jwt security key not found")))
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
    new Encoder(builder.Configuration.GetValue<string>("Salt") ??
                throw new Exception("Salt not provided")));
builder.Services.AddTransient<IDecoder, Decoder>(_ =>
    new Decoder(builder.Configuration.GetValue<string>("Salt") ??
                throw new Exception("Salt not provided")));
builder.Services.AddDbContext<SendMailDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default") ??
                      throw new Exception("Connection string not found"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(corsPolicy);

app.UseAuthorization();

app.MapControllers();

var context = app.Services.CreateScope()
    .ServiceProvider.GetRequiredService<SendMailDbContext>();

await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

app.Run();