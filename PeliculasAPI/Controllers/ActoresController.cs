using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";


        public ActoresController(ApplicationDbContext context, 
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos) 
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        // GET api/actores
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) 
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var actores = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actores);
        }

        // GET api/actores/5
        [HttpGet("{id}", Name ="obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id) 
        {
            var actor = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return  mapper.Map<ActorDTO>(actor);
        }

        // POST api/actores
        [HttpPost]
        public async Task<ActionResult>  Post([FromForm] ActorCreacionDTO actorCreacionDTO) 
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                actor.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                    actorCreacionDTO.Foto.ContentType);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(actor);

            return new CreatedAtRouteResult("obtenerActor", new { id = actor.Id }, dto);
        }

        // PUT api/actores/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                    actorDB.Foto,
                    actorCreacionDTO.Foto.ContentType);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH api/actores/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var actorDB = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB == null)
            {
                return NotFound();
            }

            var actorDTO = mapper.Map<ActorPatchDTO>(actorDB);

            patchDocument.ApplyTo(actorDTO, ModelState);

            var esValido = TryValidateModel(actorDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(actorDTO, actorDB);

            await context.SaveChangesAsync();

            return NoContent();
            
        }

        // DELETE api/actores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(a => a.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
