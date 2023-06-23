using SendMail.Data;
using SendMail.Repository;
using SendMail.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMailer, LocalMailer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<SendMailDbContext>(options =>
{
    options.UseInMemoryDatabase("tempDb");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();