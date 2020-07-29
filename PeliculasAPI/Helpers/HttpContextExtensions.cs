using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpcontext,
            IQueryable<T> queryable, int cantidadRegistrosPorPagina)
        {
            double cantidad = await queryable.CountAsync();
            double cantidadPaginas = Math.Ceiling(cantidad / cantidadRegistrosPorPagina);
            httpcontext.Response.Headers.Add("cantidadPaginas", cantidadPaginas.ToString());

        }
            
    }
}
