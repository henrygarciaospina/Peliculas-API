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
        private readonly ApplicationDbContext queryable;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";


        public ActoresController(ApplicationDbContext context, 
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos) 
        {
            this.queryable = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        // GET api/actores
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) 
        {
            var queryable = this.queryable.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(entidades);
        }

        // GET api/actores/5
        [HttpGet("{id}", Name ="obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id) 
        {
            var entidad = await queryable.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            return  mapper.Map<ActorDTO>(entidad);
        }

        // POST api/actores
        [HttpPost]
        public async Task<ActionResult>  Post([FromForm] ActorCreacionDTO actorCreacionDTO) 
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                    actorCreacionDTO.Foto.ContentType);
            }

            queryable.Add(entidad);
            await queryable.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entidad);

            return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
        }

        // PUT api/actores/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await queryable.Actores.FirstOrDefaultAsync(a => a.Id == id);

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

            await queryable.SaveChangesAsync();

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

            var entidadDB = await queryable.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<ActorPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await queryable.SaveChangesAsync();

            return NoContent();
            
        }

        // DELETE api/actores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await queryable.Actores.AnyAsync(a => a.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            queryable.Remove(new Actor() { Id = id });
            await queryable.SaveChangesAsync();

            return NoContent();
        }
    }
}
