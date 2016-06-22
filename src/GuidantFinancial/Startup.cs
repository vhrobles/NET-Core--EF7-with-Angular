using System.Threading.Tasks;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace GuidantFinancial
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            //Use Entity Framework and ApplicationDbContext as injectable in other parts of the app.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password = new PasswordOptions()
                {
                    RequireDigit = false,
                    RequireNonLetterOrDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false                    
                };
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddLogging();

            services.AddSingleton(provider => Configuration);                        
            //Each http request should get it's own instance of the object
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            //new instance each time
            services.AddTransient<SeedDbInitialData>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(
            IApplicationBuilder app,
            IHostingEnvironment environment,
            SeedDbInitialData seedDbInitialData,
            ILoggerFactory loggerFactory)
        {

            //Register middleware coming from NuGet here to be available in the application.
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIISPlatformHandler();

            //Only show app dev exception when is dev environment.
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }            
            
            //Run time info page
            app.UseRuntimeInfoPage("/info");

            //Use default static file, order is important here. All non-defined urls will go to the next middleware
            app.UseFileServer();
            app.UseIdentity();
            app.UseMvc(ConfigureRoutes);

            app.Run(async (context) =>
            {
                
                await context.Response.WriteAsync("Hello World!!");
            });
            await seedDbInitialData.EnsureSeedData();
        }

        private static void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //e.g. Home/Index/1 <-optional
            routeBuilder.MapRoute("Default", "{controller=App}/{action=Index}/{id?}");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
