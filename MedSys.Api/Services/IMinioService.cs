using Minio.DataModel.Args;

namespace MedSys.Api.Services
{
    public interface IMinioService
    {
        Task<string> GeneratePresignedFileUrl(string fileKey, int expiryInSeconds);
        Task UploadFile(string fileKey, Stream fileStream);
        Task DeleteFile(string fileKey);
    }
}
