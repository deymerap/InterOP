using AutoMapper;
using InterOP.Core.DTOs;
using InterOP.Core.Entities;

namespace InterOP.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EntProveedores, ProveedoresDto>();
            CreateMap<ProveedoresDto, EntProveedores>();
            CreateMap<EntProveedores, ProveedoresNoPwdDto>().ReverseMap();


            CreateMap<EntFacturas, FacturasDto>().ReverseMap();
            CreateMap<EntNotas, NotasDto>().ReverseMap();
            CreateMap<EntEventos, EventosDto>().ReverseMap();
        }
    }
}
