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
            CreateMap<CheckupType, CheckupTypeDTO>()
                .ReverseMap();
            CreateMap<CheckupType, CheckupTypeSimplifiedDTO>().ReverseMap();
            CreateMap<MedicalDocument, MedicalDocumentDTO>().ReverseMap();
            CreateMap<Checkup, CheckupDTO>()
                .ForMember(dest => dest.CheckupDateTime,
                    opt => opt.MapFrom(src => src.Date.ToDateTime(src.Time)))
                .ReverseMap()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CheckupDateTime)))
                .ForMember(dest => dest.Time,
                    opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.CheckupDateTime)));

            //
            CreateMap<Patient, PatientDTO>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));

            CreateMap<Patient, PatientSimplifiedDTO>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToDateTime(TimeOnly.MinValue))) 
                .ReverseMap()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));
            CreateMap<Disease, DiseaseDTO>().ReverseMap();

            CreateMap<Drug, DrugDTO>().ReverseMap();
            CreateMap<Drug, DrugSimplifiedDTO>().ReverseMap();
            CreateMap<Prescription, PrescriptionDTO>()
                        .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate.ToDateTime(TimeOnly.MinValue)))
                        .ReverseMap()
                        .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.IssueDate)));
            CreateMap<MedicalHistory, MedicalHistoryDTO>().ReverseMap();
            
        }
    }
}
