using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using MedSys.BL.DALModels;

namespace MedSys.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CheckupType, CheckupTypeDTO>().ReverseMap();
            CreateMap<MedicalDocument, MedicalDocumentDTO>().ReverseMap();
            CreateMap<Checkup, CheckupDTO>()
                .ForMember(dest => dest.CheckupDateTime,
                    opt => opt.MapFrom(src => src.Date.ToDateTime(src.Time)))
                .ReverseMap()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CheckupDateTime)))
                .ForMember(dest => dest.Time,
                    opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.CheckupDateTime)));

            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<Patient, PatientSimplifiedDTO>().ReverseMap();
            CreateMap<Disease, DiseaseDTO>().ReverseMap();

            CreateMap<Drug, DrugDTO>().ReverseMap();
            CreateMap<Prescription, PrescriptionDTO>().ReverseMap();
            CreateMap<MedicalHistory, MedicalHistoryDTO>().ReverseMap();
            
        }
    }
}
