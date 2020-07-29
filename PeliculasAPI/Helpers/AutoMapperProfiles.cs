using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                //Esto permite ignorar el mapeo de la foto
                .ForMember(a => a.Foto, options => options.Ignore());

            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                //Esto permite ignorar el mapeo de la imagen del Poster
                .ForMember(p => p.Poster, options => options.Ignore());

            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }
    }
}
