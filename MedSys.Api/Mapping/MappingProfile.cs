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
            CreateMap<Checkup, CheckupDTO>().ReverseMap();
            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<Disease, DiseaseDTO>().ReverseMap();

            CreateMap<Drug, DrugDTO>().ReverseMap();
            CreateMap<Prescription, PrescriptionDTO>().ReverseMap();
            CreateMap<MedicalHistory, MedicalHistoryDTO>().ReverseMap();
            CreateMap<MedicalDocument, MedicalDocumentDTO>().ReverseMap();
        }
    }
}
