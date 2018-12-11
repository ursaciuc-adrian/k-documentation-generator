using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
namespace DocumentationGenerator
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
				.UseKestrel(
					options =>
					{
						options.Listen(IPAddress.Any, 50003);
						options.Limits.MaxRequestBodySize = null;
					})
				.UseDefaultServiceProvider(options => { options.ValidateScopes = false; });
	}
}