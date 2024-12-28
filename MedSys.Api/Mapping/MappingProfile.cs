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
            CreateMap<CheckupType, CheckupTypeSimplifiedDTO>().ReverseMap();

            CreateMap<MedicalDocument, MedicalDocumentDTO>()
                .ReverseMap()
                .ForMember(dest => dest.FileKey, opt => opt.Ignore());

            CreateMap<Checkup, CheckupDTO>()
                .ForMember(dest => dest.CheckupDateTime,
                    opt => opt.MapFrom(src => src.Date.ToDateTime(src.Time)))
                .ReverseMap()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CheckupDateTime)))
                .ForMember(dest => dest.Time,
                    opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.CheckupDateTime)));            
            CreateMap<Checkup, CheckupSimplifiedDTO>()
                .ForMember(dest => dest.CheckupDateTime,
                    opt => opt.MapFrom(src => src.Date.ToDateTime(src.Time)))
                .ReverseMap()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CheckupDateTime)))
                .ForMember(dest => dest.Time,
                    opt => opt.MapFrom(src => TimeOnly.FromDateTime(src.CheckupDateTime)));

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
            CreateMap<Disease, DiseaseSimplifiedDTO>().ReverseMap();

            CreateMap<Drug, DrugDTO>().ReverseMap();
            CreateMap<Drug, DrugSimplifiedDTO>().ReverseMap();
            CreateMap<Prescription, PrescriptionDTO>()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.IssueDate)));            
            CreateMap<Prescription, PrescriptionSimplifiedDTO>()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.IssueDate)));
            CreateMap<MedicalHistory, MedicalHistoryDTO>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue
                    ? src.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue
                    ? DateOnly.FromDateTime(src.EndDate.Value) : (DateOnly?)null));            
            CreateMap<MedicalHistory, MedicalHistorySimplifiedDTO>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue
                    ? src.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue
                    ? DateOnly.FromDateTime(src.EndDate.Value) : (DateOnly?)null));

        }
    }
}
