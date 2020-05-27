using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;



namespace APIProject
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddSingleton<IConfiguration>(Configuration);
            HttpClient tokenClient = new HttpClient();
            services.AddSingleton<HttpClient>(tokenClient);

            Uri targetUri = new Uri(Configuration["TargetURL"]);
            ServicePointManager.FindServicePoint(targetUri).ConnectionLeaseTimeout= 60000;
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = targetUri,
            };
            services.AddSingleton<HttpClient>(httpClient);
            Console.WriteLine("TargetURL=" + targetUri);
            System.Diagnostics.Debug.WriteLine("TargetURL=" + targetUri);

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

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
        }
    }
}
