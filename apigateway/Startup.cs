
//using Microsoft.Extensions.Hosting;

using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
//using Ocelot.

namespace apigateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /* public void ConfigureServices(IServiceCollection services)
         {
             services.AddOcelot(this.Configuration);
             services.AddControllers();
         }*/

        public void ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("TokenConfigurations");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication(x =>
           {
               x.DefaultAuthenticateScheme = "TestKey";
               x.DefaultChallengeScheme = "TestKey";
           })
            .AddJwtBearer("TestKey", x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = tokenValidationParameters;

               });


            services.AddAuthentication(authOptions =>
                                  {
                                      authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                      authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                  }).AddJwtBearer(bearerOptions =>
                                  {
                                      bearerOptions.TokenValidationParameters = tokenValidationParameters;
                                  });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
               {

                   builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
               }));



            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });




            /*  services.AddAuthentication(o =>
              {
                  o.DefaultAuthenticateScheme = "TestKey";
              }) 
              .AddJwtBearer("TestKey", x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.TokenValidationParameters = tokenValidationParameters;
               });*/


            services.AddOcelot(Configuration);
        } 

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors("MyPolicy");
            //  app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
            app.UseAuthentication();
           // app.UseAuthorization();
            app.UseOcelot().Wait();
        }
    }
}
