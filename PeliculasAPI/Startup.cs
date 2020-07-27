using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeliculasAPI.Servicios;

namespace PeliculasAPI
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
            // Esta l�nea configura AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Configuraci�n servicio para subir archivos a StorageAzure
            /*
             * Si se van a subir a AzureStorage se debe descomentar la l�nea siguiente 
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
            *
            *
           */
            // Configuraci�n servicio para subir archivos al servidor
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            
            services.AddHttpContextAccessor();

            // Configuraci�n de la cadena de conexi�n
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Permite servir archivos estaticos
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
