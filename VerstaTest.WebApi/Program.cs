using VerstaTest.Application;
using VerstaTest.Application.Interfaces;

namespace VerstaTest.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        {
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddControllersWithViews();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IApplicationDbContext>(new ApplicationContext());

            builder.Services.AddApplication();
        }

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsProduction())
        {
            app.UseStaticFiles();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.MapGet("/", () => Results.Redirect("/customer/authorization/"));

        app.Run();
    }
}