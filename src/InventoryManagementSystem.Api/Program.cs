using InventoryManagementSystem.Infrastructure.Persistence;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ... existing builder setup ...
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


//For testing the mediator, we can send a PingCommand and log the result.
// using (var scope = app.Services.CreateScope())
// {
//     var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
//     var result = await mediator.Send(new PingCommand());
//     Console.WriteLine($"Mediator round-trip result: {result}");
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
