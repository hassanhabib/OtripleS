// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OtripleS.Web.Api
{
    public class Program
    {
        public static void Main(string[] arguments) =>
            CreateHostBuilder(arguments).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] arguments)
        {
            return Host.CreateDefaultBuilder(arguments)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
        }
    }
}
