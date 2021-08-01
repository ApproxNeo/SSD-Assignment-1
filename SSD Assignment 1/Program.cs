using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SSD_Assignment_1
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            Load(dotenv);
            
            if (Environment.GetEnvironmentVariable("STRIPE_KEY") is null || Environment.GetEnvironmentVariable("PUBLISHABLE_KEY") is null)
            {
                Console.WriteLine("No applicable stripe key found, refer to README on repo");
                Environment.Exit(-1);
            }

            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((Context, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('=',StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}

