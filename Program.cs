public class Program
{

    public static void Main(string[] args)
    {
    
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); // to listen for incoming http connection on port 5001
           // options.ListenAnyIP(5001, configure => configure.UseHttps()); // to listen for incoming https connection on port 5001
        });

        // Add services to the container.

        builder.Services.AddControllers();


        var app = builder.Build();

    // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }


}
