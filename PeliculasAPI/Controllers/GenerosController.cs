using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]

    public class GenerosController : CustomBaseController
    {
     
        public GenerosController(ApplicationDbContext context,
            IMapper mapper) 
            :base(context, mapper)
        {
        }

        // GET api/generos
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get() 
        {
            return await Get<Genero, GeneroDTO>();
        }

        // GET api/generos/5
        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }

        // POST api/generos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) 
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "obtenerGenero");
        }

        // PUT api/generos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        // DELETE api/generos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) 
        {
           return await Delete<Genero>(id);
        }
    }
}
