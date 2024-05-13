using Serilog;
using Serilog.Formatting.Compact;

namespace SlowlySimulate.Middleware;
public static class SerilogDI
{
    public static IHostBuilder InjectSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            var logPath = Path.Combine(Environment.CurrentDirectory, "wwwroot/Logs", "log.txt");

            loggerConfiguration
                .Enrich.FromLogContext() // To get some key information like user id/request id 
                                         //.WriteTo.Console(new JsonFormatter())
                                         //.WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                                         //.WriteTo.Seq("http://localhost:5341")
                                         //.WriteTo.File(new JsonFormatter(), "log.txt")
                .WriteTo.File(
                    formatter: new CompactJsonFormatter(),
                    path: logPath,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 20,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10000);
            //.WriteTo.MSSqlServer(configuration.GetConnectionString("DefaultConnection"),
            //    new MSSqlServerSinkOptions
            //    {
            //        TableName = "Logs",
            //        SchemaName = "dbo",
            //        AutoCreateSqlTable = true
            //    });
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
            //.CreateLogger();
        });

        return hostBuilder;
    }
}