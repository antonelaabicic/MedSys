using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using MedSys.Api.Services;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static MedSys.Api.Services.ApiRoutes;

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
        private readonly IMinioService _minioService;
        private readonly IMapper _mapper;

        public MedicalDocumentController(IRepositoryFactory repositoryFactory, IMinioService minioService, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IMedicalDocumentRepository>();
            _checkupRepository = repositoryFactory.GetRepository<ICheckupRepository>();
            _checkupTypeRepository = repositoryFactory.GetRepository<ICheckupTypeRepository>();
            _patientRepository = repositoryFactory.GetRepository<IPatientRepository>();
            _minioService = minioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDTO>>> Get()
        {
            try
            {
                var medicalDocuments = _repository.GetAll();
                var medicalDocumentsDtos = new List<MedicalDocumentDTO>();

                foreach (var document in medicalDocuments)
                {
                    var documentDto = _mapper.Map<MedicalDocumentDTO>(document);
                    documentDto.FileUrl = !string.IsNullOrEmpty(document.FileKey)
                        ? await _minioService.GeneratePresignedFileUrl(document.FileKey, 3600)
                        : null;
                    medicalDocumentsDtos.Add(documentDto);
                }

                return Ok(medicalDocumentsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalDocumentDTO>> Get(int id)
        {
            try
            {
                var medicalDocument = _repository.GetById(id);
                if (medicalDocument == null)
                {
                    return NotFound($"Medical document with id {id} was not found.");
                }

                var dto = _mapper.Map<MedicalDocumentDTO>(medicalDocument);
                if (!string.IsNullOrEmpty(medicalDocument.FileKey))
                {
                    dto.FileUrl = await _minioService.GeneratePresignedFileUrl(medicalDocument.FileKey, 3600);
                }

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("by_checkup/{checkupId}")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDTO>>> GetDocumentsByCheckup(int checkupId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByCheckupId(checkupId);
                var documentsDtos = new List<MedicalDocumentDTO>();

                foreach (var document in documents)
                {
                    var documentDto = _mapper.Map<MedicalDocumentDTO>(document);
                    documentDto.FileUrl = !string.IsNullOrEmpty(document.FileKey)
                        ? await _minioService.GeneratePresignedFileUrl(document.FileKey, 3600)
                        : null;
                    documentsDtos.Add(documentDto);
                }

                return Ok(documentsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }        
        
        [HttpGet("by_patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalDocumentDTO>>> GetDocumentsByPatient(int patientId)
        {
            try
            {
                var documents = _repository.GetMedicalDocumentByPatientId(patientId);
                var documentsDtos = new List<MedicalDocumentDTO>();

                foreach (var document in documents)
                {
                    var documentDto = _mapper.Map<MedicalDocumentDTO>(document);
                    documentDto.FileUrl = !string.IsNullOrEmpty(document.FileKey)
                        ? await _minioService.GeneratePresignedFileUrl(document.FileKey, 3600)
                        : null;
                    documentsDtos.Add(documentDto);
                }

                return Ok(documentsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] MedicalDocumentDTO documentDto, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file provided.");
                }

                var allowedExtensions = new[] { ".pdf", ".txt", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}.");
                }

                if (file.Length > 5 * 1024 * 1024)
                {
                    return BadRequest($"File {file.FileName} exceeds the size limit of 5MB.");
                }

                var checkup = _checkupRepository.GetById(documentDto.CheckupId);
                if (checkup == null)
                {
                    return NotFound($"Checkup with ID {documentDto.CheckupId} does not exist.");
                }

                var fileKey = $"checkups/{documentDto.CheckupId}/{Guid.NewGuid()}{fileExtension}";
                using var fileStream = file.OpenReadStream();
                await _minioService.UploadFile(fileKey, fileStream);

                var medicalDocument = _mapper.Map<MedicalDocument>(documentDto);
                medicalDocument.FileKey = fileKey;
                medicalDocument.CreatedAt = DateTime.Now;
                medicalDocument.Checkup = checkup;

                _repository.Insert(medicalDocument);
                _repository.Save();

                var savedDocument = _repository.GetById(medicalDocument.Id);
                var resultDto = _mapper.Map<MedicalDocumentDTO>(savedDocument);
                resultDto.FileUrl = await _minioService.GeneratePresignedFileUrl(fileKey, 3600);

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the medical document: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Put(int id, [FromForm] MedicalDocumentDTO documentDto, IFormFile file)
        {
            try
            {
                var existingMDoc = _repository.GetById(id);

                if (existingMDoc == null)
                {
                    return NotFound($"Medical document with id {id} wasn't found.");
                }

                if (file != null && file.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".txt", ".doc", ".docx" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}.");
                    }

                    if (file.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest($"File {file.FileName} exceeds the size limit of 5MB.");
                    }

                    var newFileKey = $"checkups/{documentDto.CheckupId}/{Guid.NewGuid()}{fileExtension}";
                    using var fileStream = file.OpenReadStream();
                    await _minioService.UploadFile(newFileKey, fileStream);

                    if (!string.IsNullOrEmpty(existingMDoc.FileKey))
                    {
                        await _minioService.DeleteFile(existingMDoc.FileKey);
                    }

                    existingMDoc.FileKey = newFileKey;
                }

                existingMDoc.Title = documentDto.Title ?? existingMDoc.Title;
                existingMDoc.Text = documentDto.Text ?? existingMDoc.Text;
                existingMDoc.Notes = documentDto.Notes ?? existingMDoc.Notes;
                existingMDoc.CheckupId = documentDto.CheckupId;

                _repository.Update(existingMDoc);
                _repository.Save();

                var resultDto = _mapper.Map<MedicalDocumentDTO>(existingMDoc);
                resultDto.FileUrl = !string.IsNullOrEmpty(existingMDoc.FileKey)
                    ? await _minioService.GeneratePresignedFileUrl(existingMDoc.FileKey, 3600)
                    : null;

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the medical document: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicalDocumentDTO>> Delete(int id)
        {
            try
            {
                var document = _repository.GetById(id);
                if (document.FileKey != null) 
                {
                    await _minioService.DeleteFile(document.FileKey);
                    document.FileKey = null;
                    _repository.Update(document);
                    _repository.Save();
                }

                document = _repository.Delete(id);
                var deletedMedicalDocumentDto = _mapper.Map<MedicalDocumentDTO>(document);

                return Ok(deletedMedicalDocumentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
