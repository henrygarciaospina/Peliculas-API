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
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context,
            IMapper mapper) 
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/generos
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get() 
        {
            var generos = await context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(generos);
            return dtos;
        }

        // GET api/generos/5
        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var genero= await context.Generos.FirstOrDefaultAsync(g => g.Id == id );
            if (genero == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GeneroDTO>(genero);

            return dto;
        }

        // POST api/generos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) 
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);

            await context.SaveChangesAsync();

            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return new CreatedAtRouteResult("obtenerGenero", new {id = generoDTO.Id, generoDTO });
        }

        // PUT api/generos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;
            context.Entry(genero).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/generos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(g => g.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id});
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
