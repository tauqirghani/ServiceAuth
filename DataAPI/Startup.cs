using System;
using DataAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DataAPI
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
            services.AddDbContext<FileContext>(opt => opt.UseInMemoryDatabase("Files"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    AuthenticationSettings settings = Configuration.GetSection("Authentication").Get<AuthenticationSettings>();

                    o.Authority = settings.Authority;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new[]
                        {
                            settings.ClientId,
                            settings.ApplicationIdUri
                        }
                    };
                });
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapControllers();
            });
            using (var servicescope = app.ApplicationServices.CreateScope())
            {
                var context = servicescope.ServiceProvider.GetService<FileContext>();
                AddTestData(context);
            }
        }
        private static void AddTestData(FileContext context)
        {
            var testFile1 = new File
            {
                fileId = 101,
                name = "File 101",
                fileNumber = "101101",
                status = "Open",
                totalSalesPrice = 100000.0,
                openDate = DateTime.Parse("2020-03-21T21:15:27.659Z"),
                closeDate = DateTime.Parse("2020-03-28T21:15:27.659Z"),
                transactionType = "Sale w/Mortgage"
            };

            context.Files.Add(testFile1);

            var testFile2 = new File
            {
                fileId = 202,
                name = "File 202",
                fileNumber = "202202",
                status = "Open",
                totalSalesPrice = 200000.0,
                openDate = DateTime.Parse("2020-03-21T21:15:27.659Z"),
                closeDate = DateTime.Parse("2020-03-28T21:15:27.659Z"),
                transactionType = "Sale w/Mortgage"
            };

            context.Files.Add(testFile2);

            context.SaveChanges();
        }
    }
}
