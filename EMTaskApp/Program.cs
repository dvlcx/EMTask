
using EMTaskApp.BLL;
using EMTaskApp.DAL;

namespace EMTaskApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Logging.AddFile("Logs/log-{Date}.txt"); // using Serilog.Extensions.Logging.File
        builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseAuthorization();

        app.Run();
    }
}
