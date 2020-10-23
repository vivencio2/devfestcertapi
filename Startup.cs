using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wkhtmltopdf.NetCore;

namespace devfestcertapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()));
            services.AddCors(c => c.AddPolicy("AllowAnyHeader", options => options.AllowAnyHeader()));
            services.AddCors(c => c.AddPolicy("AllowAnyMethod", options => options.AllowAnyMethod()));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddWkhtmltopdf("PDF");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseCors(options => options.AllowAnyHeader());
            app.UseCors(options => options.AllowAnyMethod());
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GDG PH API V1");
                c.RoutePrefix = string.Empty;
            });
            
        }
    }
}
