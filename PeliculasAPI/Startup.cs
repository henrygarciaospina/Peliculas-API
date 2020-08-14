using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.Helpers;
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
            // Esta línea configura AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Configuración servicio para subir archivos a StorageAzure
            /*
             * Si se van a subir a AzureStorage se debe descomentar la línea siguiente 
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
            *
            *
           */
            // Configuración servicio para subir archivos al servidor
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            
            services.AddHttpContextAccessor();

            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddSingleton(provider =>

                new MapperConfiguration(config =>
                {
                    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                    config.AddProfile(new AutoMapperProfiles(geometryFactory));
                }).CreateMapper()
            );


        // Configuración de la cadena de conexión
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            SqlServerOptions => SqlServerOptions.UseNetTopologySuite()));

            services.AddControllers()
            .AddNewtonsoftJson();
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
