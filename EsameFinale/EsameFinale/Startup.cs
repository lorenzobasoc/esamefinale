using EsameFinale.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EsameFinale.DataAccess;

namespace EsameFinale
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddScoped<ExceptionsMiddleware>();
            services.AddDbContext<ChristmasDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ConnectionString")),
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<ChristmasDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ConnectionString")),
                lifetime: ServiceLifetime.Singleton);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
