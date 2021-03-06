﻿using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistSysACW
{

    // Dependancie injection example.
    /*public interface ISomeService
    {
        void Hello(string message);
    }

    public class SomeService : ISomeService
    {
        public void Hello(string message)
        {
            Debug.WriteLine(message + "from my service");
        }
    }*/




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
            services.AddDbContext<Models.UserContext>();

            //services.AddTransient<ISomeService, SomeService>();

            // Extra control over error messages.
            services.AddMvc(options => {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add(new Filters.AuthFilter());})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Added API Key middleware to the pipeline.
            app.UseMiddleware<Middleware.AuthMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
