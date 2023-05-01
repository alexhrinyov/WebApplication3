using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Middlewares;

namespace WebApplication3
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public static IWebHostEnvironment environment;
       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            environment = env;
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            // обрабатываем ошибки HTTP
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            Console.WriteLine($"Launching project from: {env.ContentRootPath}");
            //app.Use(async (context, next) =>
            //{
            //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "RequestLog.txt");

            //    using (StreamWriter sw = new StreamWriter(filePath, true))
            //    {
            //        await sw.WriteLineAsync($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
            //    }
            //    await next.Invoke();
            //});

            //app.Use(async (context, next) =>
            //{
            //    Для логирования данных о запросе используем свойства объекта HttpContext
            //    Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
            //    await next.Invoke();
            //});

            // Метод заменяет закомментированное выше
            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {

                    await context.Response.WriteAsync($"Hello World! Configuration: {env.EnvironmentName}");
                    
                });
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/about", async context =>
            //    {
            //        await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}");
            //    });
            //});
            //Добавляем компонент для логирования запросов с использованием метода Use.
            

            app.Map("/about", About);
            app.Map("/config", Config);

            // Обработчик для ошибки "страница не найдена"
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync($"Page not found");
            //});
        }

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{environment.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  Обработчик для главной страницы
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {environment.ApplicationName}. App running configuration: {environment.EnvironmentName}");
            });
        }
    }
}
