using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Middlewares
{

    public class LoggingMiddleware

    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
           _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path;//
                string method = context.Request.Method;
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                var pathFile = "Logs/logs.txt";
                using (StreamWriter writer = File.AppendText(pathFile))
                {
                    await writer.WriteLineAsync("Path: " + path + ", Method: " + method +
                        ", Querry: " + queryString + ".");

                    await writer.WriteLineAsync("Body: " + bodyStr);
                }

            }
            if (_next != null) await _next(context);





        }



    }

}