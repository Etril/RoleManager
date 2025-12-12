using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Repositories;
using Backend.Infrastructure.Persistence.Repositories;
using Backend.Api.Middlewares;
using Backend.Api.Extensions;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddApplicationHandlers();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.SeedAsync();

app.Run();
