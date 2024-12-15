using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using MedSys.Api.Services;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalDocumentController : ControllerBase
    {
        private readonly IMedicalDocumentRepository _repository;
        private readonly ICheckupRepository _checkupRepository;
        private readonly ICheckupTypeRepository _checkupTypeRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IZipService _zipService;
        private readonly IMapper _mapper;

        public MedicalDocumentController(IRepositoryFactory repositoryFactory, IZipService zipService, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IMedicalDocumentRepository>();
            _checkupRepository = repositoryFactory.GetRepository<ICheckupRepository>();
            _checkupTypeRepository = repositoryFactory.GetRepository<ICheckupTypeRepository>();
            _patientRepository = repositoryFactory.GetRepository<IPatientRepository>();
            _zipService = zipService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MedicalDocumentDTO>> Get()
        {
            try
            {
                var medicalDocuments = _repository.GetAll();
                var medicalDocumentsDtos = _mapper.Map<IEnumerable<MedicalDocumentDTO>>(medicalDocuments);
                return Ok(medicalDocumentsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MedicalDocumentDTO> Get(int id)
        {
            var medicalDocument = _repository.GetById(id);
            if (medicalDocument == null)
            {
                return NotFound($"Medical document with id {id} was not found.");
            }

            var dto = _mapper.Map<MedicalDocumentDTO>(medicalDocument);
            if (!string.IsNullOrEmpty(medicalDocument.FilePath))
            {
                dto.FileData = Convert.ToBase64String(medicalDocument.FileData);
            }
            return Ok(dto);
        }

        [HttpGet("download/{id}")]
        public IActionResult Download(int id)
        {
            var medicalDocument = _repository.GetById(id);
            if (medicalDocument == null || medicalDocument.FileData == null)
            {
                return NotFound($"Medical document with id {id} was not found or file data is missing.");
            }

            using (var memoryStream = new MemoryStream(medicalDocument.FileData))
            {
                return File(memoryStream.ToArray(), "application/octet-stream", Path.GetFileName(medicalDocument.FilePath));
            }
        }

        [HttpPost]
        public ActionResult<MedicalDocumentDTO> Post([FromBody] MedicalDocumentDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var checkupType = _checkupTypeRepository.FindCheckupTypeByCode(value.Checkup.CheckupType.Code);
                if (checkupType == null)
                {
                    return NotFound($"Checkup type with code {value.Checkup.CheckupType.Code} does not exist.");
                }

                var patient = _patientRepository.FindPatientByDetails(value.Checkup.Patient.FirstName, value.Checkup.Patient.LastName, value.Checkup.Patient.DateOfBirth);
                if (patient == null)
                {
                    patient = _mapper.Map<Patient>(value.Checkup.Patient);
                    _patientRepository.Insert(patient);
                    _patientRepository.Save();
                    value.Checkup.Patient.Id = patient.Id;
                }

                var existingCheckup = _checkupRepository.GetExistingCheckup(patient.Id, checkupType.Id, DateOnly.FromDateTime(value.Checkup.CheckupDateTime));

                var medicalDocument = _mapper.Map<MedicalDocument>(value);
                if (existingCheckup != null)
                {
                    medicalDocument.Checkup = existingCheckup;
                }
                else 
                {
                    medicalDocument.Checkup.CheckupType = checkupType;
                    medicalDocument.Checkup.Patient = patient;
                }
                medicalDocument.CreatedAt = DateTime.Now;

                _repository.Insert(medicalDocument);
                _repository.Save();

                var savedDocument = _repository.GetById(medicalDocument.Id);
                var resultDTO = _mapper.Map<MedicalDocumentDTO>(savedDocument);

                return Ok(resultDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadFile(int id, IFormFile file)
        {
            var medicalDocument = _repository.GetById(id);
            if (medicalDocument == null)
            {
                return NotFound($"Medical document with id {id} was not found.");
            }

            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".txt", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest($"Invalid file type. Only {string.Join(", ", allowedExtensions)} are allowed.");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                medicalDocument.FileData = memoryStream.ToArray();
                medicalDocument.FilePath = file.FileName; 
            }

            _repository.Update(medicalDocument);
            _repository.Save();

            return Ok(new { message = $"{medicalDocument.FilePath} uploaded successfully." });
        }

        [HttpPut("{id}")]
        public ActionResult<MedicalDocumentDTO> Put(int id, [FromBody] MedicalDocumentDTO value)
        {
            try
            {
                var existingMDoc = _repository.GetById(id);

                if (existingMDoc == null)
                {
                    return NotFound($"Medical document with id {id} wasn't found.");
                }

                existingMDoc.Checkup.Patient.FirstName = value.Checkup.Patient.FirstName ?? existingMDoc.Checkup.Patient.FirstName;
                existingMDoc.Checkup.Patient.LastName = value.Checkup.Patient.LastName ?? existingMDoc.Checkup.Patient.LastName;
                existingMDoc.Checkup.Patient.DateOfBirth = DateOnly.FromDateTime(value.Checkup.Patient.DateOfBirth);
                existingMDoc.Checkup.Patient.Gender = value.Checkup.Patient.Gender ?? existingMDoc.Checkup.Patient.Gender;
                existingMDoc.Checkup.Patient.Oib = value.Checkup.Patient.Oib ?? existingMDoc.Checkup.Patient.Oib;
                existingMDoc.Checkup.CheckupType.Code = value.Checkup.CheckupType.Code ?? existingMDoc.Checkup.CheckupType.Code;
                existingMDoc.Checkup.CheckupType.Name = value.Checkup.CheckupType.Name ?? existingMDoc.Checkup.CheckupType.Name;
                existingMDoc.Checkup.Date = DateOnly.FromDateTime(value.Checkup.CheckupDateTime);
                existingMDoc.Checkup.Time = TimeOnly.FromDateTime(value.Checkup.CheckupDateTime);
                existingMDoc.Title = value.Title ?? existingMDoc.Title;
                existingMDoc.Text = value.Text ?? existingMDoc.Text;
                existingMDoc.Notes = value.Notes ?? existingMDoc.Notes;

                _repository.Update(existingMDoc);
                _repository.Save();

                value.Id = existingMDoc.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<MedicalDocumentDTO> Delete(int id)
        {
            try
            {
                var deletedMedicalDocument = _repository.Delete(id);
                if (deletedMedicalDocument == null)
                {
                    return NotFound($"Medical document with id {id} wasn't found.");
                }

                var deletedMedicalDocumentDto = _mapper.Map<MedicalDocumentDTO>(deletedMedicalDocument);

                return Ok(deletedMedicalDocumentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("checkup/{checkupId}/download")]
        public IActionResult DownloadDocumentsByCheckup(int checkupId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByCheckupId(checkupId).ToList();
                if (!documents.Any())
                {
                    return NotFound($"No medical documents found for checkup with ID {checkupId}.");
                }

                var patient = documents.FirstOrDefault()?.Checkup.Patient;
                var zipFileName = $"{patient.FirstName}_{patient.LastName}_{documents.FirstOrDefault().Checkup.Date:yyyyMMdd}.zip";

                return _zipService.CreateZipFile(documents, zipFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("patient/{patientId}/download")]
        public IActionResult DownloadDocumentsByPatient(int patientId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByPatientId(patientId).ToList();
                if (!documents.Any())
                {
                    return NotFound($"No medical documents found for patient with ID {patientId}.");
                }

                var patient = documents.FirstOrDefault()?.Checkup.Patient;
                var zipFileName = $"{patient.FirstName}_{patient.LastName}.zip";

                return _zipService.CreateZipFile(documents, zipFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("checkup/{checkupId}")]
        public IActionResult GetDocumentsMetadataByCheckup(int checkupId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByCheckupId(checkupId).ToList();
                if (!documents.Any())
                {
                    return NotFound($"No medical documents found for checkup with ID {checkupId}.");
                }

                var documentDtos = _mapper.Map<IEnumerable<MedicalDocumentDTO>>(documents);
                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("patient/{patientId}")]
        public IActionResult GetDocumentsMetadataByPatient(int patientId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByPatientId(patientId).ToList();
                if (!documents.Any())
                {
                    return NotFound($"No medical documents found for patient with ID {patientId}.");
                }

                var documentDtos = _mapper.Map<IEnumerable<MedicalDocumentDTO>>(documents);
                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
