using Microsoft.EntityFrameworkCore;
using OppgaveUkeEnModul3.Core;
using OppgaveUkeEnModul3.Core.Interfaces;
using OppgaveUkeEnModul3.Core.Services;
using OppgaveUkeEnModul3.WebApi.Services;
using OppgaveUkeEnModul3.WebApi.DatabaseContext;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddControllers();


builder.Services.AddScoped<LevelCalculator>();
builder.Services.AddScoped<FightService>();
builder.Services.AddScoped<GameService>();

builder.Services.AddDbContext<StoreMonstersContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IStoreMonstersRepository>(
    provider => provider.GetRequiredService<StoreMonstersContext>());

builder.Services.AddScoped<IStoreMonstersService, StoreMonstersService>();
builder.Services.AddTransient<StoreMonsterBuilder>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider
        .GetRequiredService<StoreMonstersContext>();

    database.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();


