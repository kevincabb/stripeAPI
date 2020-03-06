using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyStore.Context;
using MyStore.Services;

namespace MyStore
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			var connectionString = Configuration.GetConnectionString("ConnectionLocal");

			services.AddDbContext<StoreDataContext>(options => options.UseSqlServer(connectionString));

			services.AddSingleton<InventoryFixedDataService>();
			services.AddSingleton<LiteDatabase>(new LiteDatabase(@"Filename=./Data/LiteDb.db;Mode=Shared"));

			services.AddScoped<InventoryLiteDbService>();
			services.AddScoped<DataService>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
