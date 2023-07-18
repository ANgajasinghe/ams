using ams;
using ams.Interfaces;
using ams.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ams",
        Version = "v1"
    });
});

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextPool<AppDbContext>((serviceProvider, optionsBuilder) =>
{
    optionsBuilder.UseInternalServiceProvider(serviceProvider);
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.LogTo(Console.WriteLine);
});

builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IEmployeeAllowanceService, EmployeeAllowanceService>();
builder.Services.AddScoped<IUploadHistoryService, UploadHistoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();