using Microsoft.OpenApi;
using Swashbuckle.AspNetCore;
using Microsoft.Extensions.DependencyInjection;


namespace Backend.Api.Extensions; 

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        var schemeId= "Bearer";
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title= "RoleManagerAPI",
                Version = "v1"
            });

            options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
            {
                Type= SecuritySchemeType.Http,
                Scheme= "bearer",
                BearerFormat="JWT",
                Description = "JWT Auth header with Bearer scheme"
                
            });

             options.AddSecurityRequirement(document =>
{
    var requirement = new OpenApiSecurityRequirement
      {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        
        };
    
     return requirement;
     });
     
     });
     
      return services;
    }
    
}