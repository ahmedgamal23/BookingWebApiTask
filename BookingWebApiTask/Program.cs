
using NSwag.Generation.Processors.Security;

namespace BookingWebApiTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddOpenApiDocument(option =>
            {
                option.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
                {
                    Description = "Bearer token authorization header",
                    Type = NSwag.OpenApiSecuritySchemeType.Http,
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });
                option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
