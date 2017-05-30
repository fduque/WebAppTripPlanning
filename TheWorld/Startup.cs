using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld
{

    public class Startup
    {

        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath) // aqui informamos o local que o arquivo do json estara...falamos que esta no diretorio raiz da applicação
                .AddJsonFile("config.json")
                .AddEnvironmentVariables(); //podemos por exemplo adicionar variaveis de ambiente..aqui esta sobrescrevendo o json
            _config = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);


            if (_env.IsDevelopment())
            {
                services.AddScoped<IMailService, DebugMailService>(); //AQUI pede-se que qd necessario o servilo "Imailservice"... seja instanciado...
                
            } else
            {
                //implement a real service
            }

            services.AddDbContext<WorldContext>(); //adicionando o servico de persistencia ao projeto
            services.AddScoped<IWorldRepository, WorldRepository>(); //injetando 
            services.AddTransient<GeoCoordsService>(); //injetando serviço de busca de geolocalizacao
            services.AddIdentity<WorldUser, IdentityRole>(config =>
                {
                    //aqui podemos aumentar o rigor ou não sobre as regras de autenticacao no sistema
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequiredLength = 8;
                    config.Cookies.ApplicationCookie.LoginPath = "/auth/login";
                    config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = async ctx =>
                        {
                            //aqui definimos como o sistema entende que é uma chamada de API...
                            // Quando a url chamada tiver um trech de "/api" e quando o retorno for "200", ou seja, está tudo bem... autorizamos o 401
                            if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                               
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            await Task.Yield();
                        }
                    };

                })
                .AddEntityFrameworkStores<WorldContext>();//injetando servico de gestão de identidade

            services.AddLogging(); //injetando serviço de logging
            services.AddTransient<WorldContextSeedData>(); //injetando dados iniciais no sistema
             //injetando pattern MVC ao sistema
            services.AddMvc(config=>
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());//definindo que todo site so podera ser acessado atraves de protocolo seguro
                }

            })
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,WorldContextSeedData seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap(); // o reverse map permite criar um viewmodel atraves de uma trip existente...
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
               
            });
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information);
                app.UseDeveloperExceptionPage(); //injeta a funcao de mostrar a lista de erros na tela
            } else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseIdentity(); //após configurar la em cima, aqui pedimos ao sistema para usar o serviço

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                    );
            });
            seeder.EnsureSeedData().Wait();

            //app.UseDefaultFiles();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
