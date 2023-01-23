using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VAT_TODOLIST
{
    public class Program
    {
        public static void Main(string[] args)
        {


            //CreateHostBuilder(args).Build().Run();
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            ConfigurationLogger();

            Log.Information(messageTemplate:"Application Starting Up");
            try
            {
                
                CreateHostBuilder(args).Build().Run();
            }
            //catch (Exception Ex)
            //{
            //    //Log.Error(Ex, "This Application failed to Start");
            //    throw;
            //}
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });

        public static void ConfigurationLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path:@"log.txt")               
               .CreateLogger();
        }
    }
}
