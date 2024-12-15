using MedSys.BL.DALModels;
using Microsoft.AspNetCore.Mvc;

namespace MedSys.Api.Services
{
    public interface IZipService
    {
        FileContentResult CreateZipFile(IEnumerable<MedicalDocument> documents, string zipFileName);
    }
}
