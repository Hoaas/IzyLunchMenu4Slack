using Api.Config;
using Api.ImageSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Startup>(optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureCognitiveConfig>(opt => Configuration.GetSection("AzureCognitive").Bind(opt));

            services.AddMemoryCache();

            services.AddHttpClient();

            services.AddTransient<IHelsedirMenuFetcher, HelsedirMenuFetcher>();
            services.AddTransient<IHelsedirMenuService, HelsedirMenuService>();
            services.AddTransient<IImageSearcher, ImageSearcher>();

            services.AddRouting();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(o =>
            {
                o.MapControllers();
            });
        }
    }
}
