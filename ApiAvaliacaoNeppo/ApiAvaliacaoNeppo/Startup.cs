using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositorio;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiAvaliacaoNeppo
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
            RegisterServices(services);
            RegisterContext(services);

            services.AddMvc();

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "API Avaliação Neppo",
                    Version = "v1"
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ApiAvaliacaoNeppo.xml"));
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<Servicos.Servicos>();
        }

        private void RegisterContext(IServiceCollection services)
        {
            services.AddDbContext<PessoaContext>(opt => opt.UseInMemoryDatabase("Pessoa"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Avaliação Neppo");
            });

            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
