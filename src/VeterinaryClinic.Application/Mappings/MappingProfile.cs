

using AutoMapper;
using VeterinaryClinic.Application.DTOs.Appointment;
using VeterinaryClinic.Application.DTOs.Owner;
using VeterinaryClinic.Application.DTOs.Pet;
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.GetFullName()))
                 .ForMember(dest => dest.PetCount, opt => opt.MapFrom(src => src.Pets != null ? src.Pets.Count : 0));


            CreateMap<CreateOwnerDto, Owner>();
            CreateMap<UpdateOwnerDto, Owner>();

            CreateMap<Pet, PetDto>()
                 .ForMember(dest => dest.AgeInYears, opt => opt.MapFrom(src => src.GetAgeInYears()))
                 .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.GetFullName() : string.Empty));

            CreateMap<CreatePetDto, Pet>();
            CreateMap<UpdatePetDto, Pet>();


            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.CanBeCancelled, opt => opt.MapFrom(src => src.CanBeCancelled()))
                .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.Pet != null? src.Pet.Name : string.Empty))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Pet != null && src.Pet.Owner != null ? src.Pet.Owner.GetFullName() : string.Empty) );

            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Scheduled"));

            CreateMap<UpdateAppointmentDto, Appointment>();

        }
    }
}