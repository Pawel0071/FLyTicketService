using AutoMapper;
using FLyTicketService.DTO;
using FLyTicketService.Model;

namespace FLyTicketService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapowanie DTO na model EF
            CreateMap<TicketDTO, Ticket>()
               .ForMember(dest => dest.TicketId, opt => opt.Ignore())
               .ForMember(dest => dest.FlightSeat, opt => opt.Ignore())
               .ForMember(dest => dest.Tenant, opt => opt.Ignore())
               .ForMember(dest => dest.ReleaseDate, opt => opt.Ignore());

            // Mapowanie modelu EF na DTO
            CreateMap<Ticket, TicketDTO>()
               .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.FlightSeat.SeatNumber))
               .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.Tenant.TenantId))
               .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.HasValue ? src.ReleaseDate.Value.DateTime : (DateTime?)null));

            // Mapowanie DTO na model EF
            CreateMap<TenantDTO, Tenant>()
               .ForMember(dest => dest.TenantId, opt => opt.Ignore()); 

            // Mapowanie modelu EF na DTO
            CreateMap<Tenant, TenantDTO>();

        }
    }
}
