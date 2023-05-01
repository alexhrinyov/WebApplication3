using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace WebApplication3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        ///  Middleware-компонент должен иметь конструктор, принимающий RequestDelegate
        /// </summary>
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///  Необходимо реализовать метод Invoke  или InvokeAsync
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Для логирования данных о запросе используем свойста объекта HttpContext
            Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "RequestLog.txt");

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                await sw.WriteLineAsync($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
            }

            // Передача запроса далее по конвейеру
            await _next.Invoke(context);
        }
    }
}
