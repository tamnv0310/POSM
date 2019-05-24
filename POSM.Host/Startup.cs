using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POSM.Data;
using POSM.Domain.User;
using POSM.Host.Filter;
using POSM.Host.Middlewares;
using POSM.Services.Interfact.Users;
using POSM.Services.Services.Users;
using Swashbuckle.AspNetCore.Swagger;

namespace POSM.Host
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
            // add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), opt => opt.UseRowNumberForPaging()));

            services.AddIdentity<User, CustomRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.AddSecurityDefinition("userToken",
                     new ApiKeyScheme
                     {
                         In = "header",
                         Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                         Name = "Authorization",
                         Type = "apiKey"
                     });
            });

            //Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policyBuilder => policyBuilder.AddRequirements(new CheckHeaderToken()));
            });
            services.AddTransient<IAuthorizationHandler, CheckHeaderTokenHandler>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            app.UseCors("CorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
