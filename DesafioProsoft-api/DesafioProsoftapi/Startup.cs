using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using WebApiFrameWork.auth;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using WebApiFrameWork.util;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Text.Json.Serialization;

namespace Workflowapi
{
 public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }
        private const string MyAllowSpecificOrigins = "_MyPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
  
            //services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            //IServiceCollection serviceCollection = services.AddSingleton<ILiteDbContext, LiteDbContext>();
            //services.AddTransient<IUserAuthService, UserAuthService>();

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

        /*   services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
               {

                   builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
               }));
           */




            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);
            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfigurations.Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = tokenConfigurations.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenConfigurations.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(authOptions =>
                        {
                            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(bearerOptions =>
                        {
                            bearerOptions.TokenValidationParameters = tokenValidationParameters;
                            // var paramsValidation = bearerOptions.TokenValidationParameters;
                            // paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                            // paramsValidation.ValidAudience = tokenConfigurations.Audience;
                            // paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                            // // Valida a assinatura de um token recebido
                            // paramsValidation.ValidateIssuerSigningKey = true;

                            // // Verifica se um token recebido ainda é válido
                            // paramsValidation.ValidateLifetime = true;

                            // // Tempo de tolerância para a expiração de um token (utilizado
                            // // caso haja problemas de sincronismo de horário entre diferentes
                            // // computadores envolvidos no processo de comunicação)
                            // paramsValidation.ClockSkew = TimeSpan.Zero;

                        });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
    



            services.AddMvc();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //  options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddControllers();
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            /*  if (env.)
              {
                  app.UseDeveloperExceptionPage();
              }*/
            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());



            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"resources")),
                RequestPath = new PathString("/resources")
            });


            

           // app.UseAuthentication();
          //  app.UseIPFilter();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}