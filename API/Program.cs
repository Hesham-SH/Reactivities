using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDatabaseContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabaseContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;

try
{
    var context = service.GetRequiredService<ApplicationDatabaseContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An Error Was Made During The Migration");
}


app.Run();
