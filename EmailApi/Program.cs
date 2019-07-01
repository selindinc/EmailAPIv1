using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmailApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseKestrel()
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       IHostingEnvironment env = hostingContext.HostingEnvironment;

                       string environmentName = env.EnvironmentName;
                       string basePath = Directory.GetCurrentDirectory();

                       Console.WriteLine("EnvironmentName : " + environmentName);
                       Console.WriteLine("Env Base Path : " + basePath);

                       config.SetBasePath(basePath)
                             .AddJsonFile("appsettings.json");

                       config.AddEnvironmentVariables();
                   });

    }
}
