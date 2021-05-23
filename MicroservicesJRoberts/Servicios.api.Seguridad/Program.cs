using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            var hostServer = CreateHostBuilder(args).Build();

            using(var contexto = hostServer.Services.CreateScope())
            {
                var services = contexto.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();
                    var contextoEF = services.GetRequiredService<SeguridadContexto>();

                    SeguridadData.InsertarUsuario(contextoEF, userManager).Wait();
                }
                catch (Exception e)
                {
                    var loggin = services.GetRequiredService<ILogger<Program>>();
                    loggin.LogError(e, "error cuando registra usuario");
                }
            }

            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
