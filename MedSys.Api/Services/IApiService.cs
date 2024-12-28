using MedSys.Api.Dtos;
using MedSys.Api.DTOs;

namespace MedSys.Api.Services
{
    public interface IApiService
    {
        Task<List<T>> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<HttpResponseMessage> CreateAsync<T>(T entity);
        Task<bool> UpdateAsync<T>(int id, T entity);
        Task<bool> DeleteAsync<T>(int id);
        Task<List<T>> GetItemsByPatientIdAsync<T>(int patientId);
        Task<List<PatientDTO>> SearchPatientsAsync(string? lastName = null, string? oib = null);
        Task<string> ExportPatientsAsync();
        Task<List<MedicalDocumentDTO>> GetDocumentsByCheckupIdAsync(int checkupId);
        Task<HttpResponseMessage> CreateMedicalDocumentAsync(MultipartFormDataContent content);
        Task<HttpResponseMessage> EditMedicalDocumentAsync(int medicalDocumentId, MultipartFormDataContent content);
    }
}
