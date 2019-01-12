using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RV.Web.Configuration;
using RV.Web.Repository.Point;
using RV.Web.Repository.Road;

namespace RV.Web
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
            var cors = Configuration.GetSection("CorsConfigurationSection").Get<CorsConfigurationSection>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RV.API",
                    Description = "API for RV - Roads and Views"
                });
            });
            services.Configure<PostgresConfiguration>(Configuration.GetSection("postgresConfiguration"));
            services.AddSingleton<IPointRepository, PointRepository>();
            services.AddSingleton<IRoadRepository, RoadRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RV.API V1"); });

            AutoMapperConfiguration.Configure();

           // app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors("MyPolicy");
        }
    }
}