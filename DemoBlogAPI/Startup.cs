using DemoBlog.Core;
using DemoBlog.Framework.Integrations.MediatR;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlogAPI
{
    public class Startup
    {
        public IWebHostEnvironment CurrentEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoBlogAPI", Version = "v1" });
            });

            string dbName = "BlogDB";
            if (CurrentEnvironment.EnvironmentName == "Testing")
                dbName = "BlogDB_Integraiton";

            services.AddMediatR(typeof(DemoBlog.Services.Features.Blogs.CreateBlogPost), typeof(MediatorPipelineHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatorPipelineHandler<,>));
            services.AddDbContext<BlogContext>(options => options.UseInMemoryDatabase(databaseName: dbName));
            services.ConfigureSwaggerGen(options =>
            {
                //your custom configuration goes here
                // UseFullTypeNameInSchemaIds replacement for .NET Core
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoBlogAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BlogContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
