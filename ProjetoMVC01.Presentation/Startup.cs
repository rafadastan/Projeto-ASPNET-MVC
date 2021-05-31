using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoMVC01.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation
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
            //configurando o projeto para o padrão de rotas do MVC!
            services.AddControllersWithViews();

            #region Configuração do modo de autenticação

            //habilitando o projeto NET CORE MVC para usar cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                //não permite compartilhamento de cookies com outros sites..
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            //configurando o modo de autenticação do NET CORE MVC
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            #endregion

            #region Injeção de dependência

            //capturar a string de conexão do banco de dados (/appsettings.json)
            var connectionstring = Configuration.GetConnectionString("ProjetoMVC01");

            //injetar o valor da connectionstring nas classes da camada de repositorio
            services.AddTransient(map => new FornecedorRepository(connectionstring));
            services.AddTransient(map => new ProdutoRepository(connectionstring));
            services.AddTransient(map => new UsuarioRepository(connectionstring));

            #endregion

            #region Configuração de Sessions

            services.AddSession(
                options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(15);
                    options.Cookie.HttpOnly = true;
                }
                );

            //objeto utilizado para manipular as sessions nas páginas .cshtml (Razor)
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy(); //habilitando!
            app.UseAuthentication(); //habilitando!
            app.UseAuthorization(); //habilitando!

            app.UseSession(); //habilitando!

            app.UseEndpoints(endpoints =>
            {
                //definindo a página inicial do projeto!
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Account}/{action=Login}"
                    );
            });
        }
    }
}
