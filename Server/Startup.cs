using BLL.DapperRepo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Server.Middleware;

namespace Server
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
            // string connectionString = "Server=localhost;Database=Contoso_Authentication_Logs;User Id=sa;Password=Password123;";
            string connectionString = "Server=db;Database=Contoso_Authentication_Logs;User Id=sa;Password=Your_password123;";

            services.AddTransient<IRegistrationsRepository, RegistrationsRepository>(provider => new RegistrationsRepository(connectionString));
            services.AddTransient<ISessionsRepository, SessionsRepository>(provider => new SessionsRepository(connectionString));
            services.AddTransient<IAnomaliesRepository, AnomaliesRepository>(provider => new AnomaliesRepository(connectionString));

            
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}