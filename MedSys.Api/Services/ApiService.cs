using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;

namespace MedSys.Api.Services
{
    public class ApiService : IApiService
    {
        private const string CLIENT = "MedSys.Api";
        private readonly HttpClient _client;

        private readonly Dictionary<Type, string> _baseRoutes = new()
        {
            { typeof(CheckupSimplifiedDTO), ApiRoutes.Checkup.Base }, { typeof(CheckupDTO), ApiRoutes.Checkup.Base },
            { typeof(CheckupTypeDTO), ApiRoutes.CheckupType.Base }, { typeof(CheckupTypeSimplifiedDTO), ApiRoutes.CheckupType.Base },
            { typeof(DrugDTO), ApiRoutes.Drug.Base }, { typeof(DiseaseDTO), ApiRoutes.Disease.Base }, 
            { typeof(PatientDTO), ApiRoutes.Patient.Base }, { typeof(PrescriptionDTO), ApiRoutes.Prescription.Base }, 
            { typeof(PrescriptionSimplifiedDTO), ApiRoutes.Prescription.Base }, 
            { typeof(MedicalHistoryDTO), ApiRoutes.MedicalHistory.Base }, 
            { typeof(MedicalHistorySimplifiedDTO), ApiRoutes.MedicalHistory.Base },
            { typeof(MedicalDocumentDTO), ApiRoutes.MedDocument.Base }
        };
        public ApiService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(CLIENT);
        }

        private string GetRouteForType<T>()
        {
            if (_baseRoutes.TryGetValue(typeof(T), out var route))
            {
                return route;
            }
            throw new Exception($"Route not defined for type {typeof(T).Name}");
        }
        public async Task<List<T>> GetAllAsync<T>()
        {
            var route = GetRouteForType<T>();
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<T>>();
                return data ?? new List<T>();
            }
            throw new Exception($"Error fetching data from {route}");
        }
        public async Task<T> GetByIdAsync<T>(int id)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return data ?? Activator.CreateInstance<T>();
            }
            throw new Exception($"Error fetching data from {route}");
        }
        public async Task<HttpResponseMessage> CreateAsync<T>(T entity)
        {
            var route = GetRouteForType<T>();
            return await _client.PostAsJsonAsync(route, entity);
        }
        public async Task<bool> UpdateAsync<T>(int id, T entity)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.PutAsJsonAsync(route, entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync<T>(int id)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.DeleteAsync(route);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<T>> GetItemsByPatientIdAsync<T>(int patientId)
        {
            var route = $"{GetRouteForType<T>()}/patient/{patientId}";
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<T>>();
                return data ?? new List<T>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<T>(); 
            }
            throw new Exception($"Error fetching for patient ID {patientId} from {route}");
        }

        public async Task<List<PatientDTO>> SearchPatientsAsync(string? lastName = null, string? oib = null)
        {
            var route = $"{ApiRoutes.Patient.Search}"; 
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(lastName))
                queryParams["lastName"] = lastName;
            if (!string.IsNullOrWhiteSpace(oib))
                queryParams["oib"] = oib;

            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            if (!string.IsNullOrEmpty(queryString))
                route += $"?{queryString}";

            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<PatientDTO>>();
                return data ?? new List<PatientDTO>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PatientDTO>(); 
            }

            throw new Exception($"Error fetching data from {route}");
        }
        public Task<string> ExportPatientsAsync()
        {
            var route = $"{_client.BaseAddress}{ApiRoutes.Patient.Export}";
            return Task.FromResult(route);
        }
        public async Task<List<MedicalDocumentDTO>> GetDocumentsByCheckupIdAsync(int checkupId)
        {
            var response = await _client.GetAsync($"{ApiRoutes.MedDocument.SearchByCheckup}/{checkupId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<MedicalDocumentDTO>>();
                return data ?? new List<MedicalDocumentDTO>();
            }
            throw new Exception($"Error fetching medical documents by checkup.");
        }

        public async Task<HttpResponseMessage> CreateMedicalDocumentAsync(MultipartFormDataContent content)
        {
            return await _client.PostAsync($"{ApiRoutes.MedDocument.Base}", content);
        }
        public async Task<HttpResponseMessage> EditMedicalDocumentAsync(int medicalDocumentId, MultipartFormDataContent content)
        {
            return await _client.PutAsync($"{ApiRoutes.MedDocument.Base}/{medicalDocumentId}", content);
        }
    }
}
