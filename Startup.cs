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
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

namespace MyStore {
	public class Startup {
		public Startup (IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices (IServiceCollection services) {
			services.AddControllers ()
				.AddJsonOptions (options => options.JsonSerializerOptions.IgnoreNullValues = true);

			services.AddMvc ();
			services.AddSwaggerGen (c => {
				c.SwaggerDoc ("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
			});

			services
				.AddScoped<DataService> ();

			// Stripe
			// See your keys here: https://dashboard.stripe.com/account/apikeys
			Stripe.StripeConfiguration.ApiKey = Configuration
				.GetSection ("Stripe").GetValue ("ApiKey", "");

			services
				.AddTransient<StripeHelperService> ()
				.AddTransient<SessionService> ()
				.AddTransient<PaymentIntentService> ();

			var connString = Configuration.GetConnectionString ("MyDatabase");
			services.AddDbContext<DataContext> (options => options.UseSqlServer (connString));

			services.AddCors (options => {
				options.AddPolicy ("CorsPolicy",
					builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
					// .AllowCredentials ());
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment ()) {
				app.UseDeveloperExceptionPage ();
			}

			app.UseSwagger ();
			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI (c => {
				c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseRouting ();
			app.UseAuthorization ();
			app.UseCors ("CorsPolicy");
			app.UseEndpoints (endpoints => {
				endpoints.MapControllers ().RequireCors ("CorsPolicy");
			});

			
		}
	}
}