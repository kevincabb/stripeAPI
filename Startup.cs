using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyStore.Services;
using MyStore.Services.Context;
using Stripe;
using Stripe.Checkout;

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
			services.AddControllers()
				.AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

			services
				.AddSingleton<InventoryFixedDataService>()
				.AddSingleton<LiteDatabase>(new LiteDatabase(@"Filename=./Data/LiteDb.db;Mode=Shared"));

			services
				.AddScoped<InventoryLiteDbService>()
				.AddScoped<DataService>();

			// Stripe
			// See your keys here: https://dashboard.stripe.com/account/apikeys
			Stripe.StripeConfiguration.ApiKey = Configuration
				.GetSection("Stripe").GetValue("ApiKey", "");

			services
				.AddTransient<StripeHelperService>()
				.AddTransient<SessionService>()
				.AddTransient<PaymentIntentService>();


			var connString = Configuration.GetConnectionString("MyDatabase");
			services.AddDbContext<DataContext>(options => options.UseSqlServer(connString));

			services.AddCors(options =>
				{
					options.AddPolicy("CorsPolicy",
						builder => builder.WithOrigins("http://localhost:4200")
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials());
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthorization();
			app.UseCors("CorsPolicy");
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
