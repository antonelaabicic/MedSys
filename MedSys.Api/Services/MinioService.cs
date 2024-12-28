using Minio;
using Minio.DataModel.Args;

namespace MedSys.Api.Services
{
    public class MinioService : IMinioService
    {
        private readonly IMinioClient _minioClient;
        private const string MinioBaseUrl = "https://regoch.net:9000"; 
        private const string BucketName = "antonela-aabicic-p1";

        public MinioService()
        {
            _minioClient = new MinioClient()
                .WithCredentials("minioAdmin", "supersecretpassword")
                .WithEndpoint("regoch.net:9000")
                .Build();
        }
        public async Task<string> GeneratePresignedFileUrl(string fileKey, int expiryInSeconds)
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileKey)
                .WithExpiry(expiryInSeconds);

            return await _minioClient.PresignedGetObjectAsync(args);
        }
        public async Task UploadFile(string fileKey, Stream fileStream)
        {
            try
            {
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileKey)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType("application/octet-stream")); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file to MinIO: {ex.Message}", ex);
            }
        }
        public async Task DeleteFile(string fileKey)
        {
            try
            {
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileKey));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file from MinIO: {ex.Message}", ex);
            }
        }
    }
}
