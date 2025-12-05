using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Repositories;
using Backend.Infrastructure.Persistence.Repositories;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
