﻿using AutoMapper;
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
            var entidades = await context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        // GET api/generos/5
        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await context.Generos.FirstOrDefaultAsync(g => g.Id == id );
            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GeneroDTO>(entidad);

            return dto;
        }

        // POST api/generos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) 
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(entidad);

            await context.SaveChangesAsync();

            var generoDTO = mapper.Map<GeneroDTO>(entidad);

            return new CreatedAtRouteResult("obtenerGenero", new {id = generoDTO.Id, generoDTO });
        }

        // PUT api/generos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/generos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = context.Generos.AnyAsync(g => g.Id == id);

            if (existe == null)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id});
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
