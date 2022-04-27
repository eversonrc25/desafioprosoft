using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace apigateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        //   public static IHostBuilder CreateHostBuilder(string[] args) =>
 
        //       Host.CreateDefaultBuilder(args)
        //           .ConfigureAppConfiguration(ic =>
        //           {
        //               ic.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

        //               ic.AddJsonFile("hostsettings.json", optional: false, reloadOnChange: true);

        //           })
        //               .ConfigureWebHostDefaults(webBuilder =>
        //               {
        //                   webBuilder.UseStartup<Startup>();
        //               });


                         public static IWebHost BuildWebHost(string[] args) {

         var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("configuration.json", optional: true, reloadOnChange: true)
                     .AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true)
                    
                    .AddCommandLine(args)
                    .Build();

           

          return  WebHost.CreateDefaultBuilder(args)
          .UseUrls("http://*:5000")
            .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseKestrel(options=>{ options.Limits.MaxRequestBodySize = null; })
                .Build();
        }
    }
}
