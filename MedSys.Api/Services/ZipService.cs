using MedSys.BL.DALModels;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace MedSys.Api.Services
{
    public class ZipService : IZipService
    {
        public FileContentResult CreateZipFile(IEnumerable<MedicalDocument> documents, string zipFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in documents)
                    {
                        if (document.FileData != null && !string.IsNullOrEmpty(document.FilePath))
                        {
                            var fileName = Path.GetFileName(document.FilePath);
                            var zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                            using (var zipStream = zipEntry.Open())
                            {
                                zipStream.Write(document.FileData, 0, document.FileData.Length);
                            }
                        }
                    }
                }

                return new FileContentResult(memoryStream.ToArray(), "application/zip")
                {
                    FileDownloadName = zipFileName
                };
            }
        }
    }
}
