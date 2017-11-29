using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ggcvan
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isDevelopment = env.Equals(EnvironmentName.Development);
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls((isDevelopment)? "http://localhost:54321" : "http://localhost:5000")
                .Build();
        }
    }
}
