//using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanlogicAI.Controllers;
using PlanlogicAI.Data;
using PlanlogicAI.Extensions;
using PlanlogicAI.Mapping;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Unity;

namespace PlanlogicAI
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
      var config = new AutoMapper.MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new MappingProfile());
      });

      var mapper = config.CreateMapper();
      services.AddSingleton(mapper);
      //services.AddAutoMapper();
      services.AddDbContext<StrategyOptimizerPrototypeContext>(options =>
  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


    //  services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));


      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      Controllers.ApiHelper.InitializeClient();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
      });


      // In production, the Angular files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
               // app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

      //var hangfireContainer = new UnityContainer();
      //GlobalConfiguration.Configuration.UseActivator(new UnityJobActivator(hangfireContainer));
      //app.UseHangfireServer();
      //app.UseHangfireDashboard();


      app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                   // spa.UseProxyToSpaDevelopmentServer("https://localhost:4200");
                }
            });

      app.UseSwagger();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

    //  RecurringJob.AddOrUpdate<APIUpdate>(x => x.SendAsync(), Cron.Daily);

    }


  }

  //public class UnityJobActivator : JobActivator
  //{
  //  private readonly IUnityContainer hangfireContainer;

  //  public UnityJobActivator(IUnityContainer hangfireContainer)
  //  {
  //    this.hangfireContainer = hangfireContainer;
  //    //this.hangfireContainer.RegisterType(new PerResolveLifetimeManager());
  //    ////don't forget to register child dependencies as well
  //  }


  //  public override object ActivateJob(Type type)
  //  {
  //    return hangfireContainer.Resolve(type);
  //  }
  //}
}
