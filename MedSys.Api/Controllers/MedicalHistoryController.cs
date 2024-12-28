using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IRepository<Disease> _diseaseRepository;
        private readonly IMapper _mapper;

        public MedicalHistoryController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IMedicalHistoryRepository>();
            _patientRepository = repositoryFactory.GetRepository<IPatientRepository>();
            _diseaseRepository = repositoryFactory.GetRepository<IRepository<Disease>>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MedicalHistoryDTO>> Get()
        {
            try
            {
                var medicalHistories = _repository.GetAll();
                var medicalHistoryDtos = _mapper.Map<IEnumerable<MedicalHistoryDTO>>(medicalHistories);
                return Ok(medicalHistoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MedicalHistoryDTO> Get(int id)
        {
            var medicalHistory = _repository.GetById(id);
            if (medicalHistory == null)
            {
                return NotFound($"Medical history with id {id} was not found.");
            }

            var dto = _mapper.Map<MedicalHistoryDTO>(medicalHistory);

            return Ok(dto);
        }

        [HttpGet("patient/{patientId}")]
        public ActionResult<IEnumerable<MedicalHistoryDTO>> GetByPatientId(int patientId)
        {
            var medicalHistories = _repository.GetMedicalHistoryByPatientId(patientId);
            if (!medicalHistories.Any())
            {
                return NotFound($"No medical history found for patient with id {patientId}.");
            }

            var dtos = medicalHistories.Select(p => _mapper.Map<MedicalHistoryDTO>(p)).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<MedicalHistoryDTO> Post([FromBody] MedicalHistorySimplifiedDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var patient = _patientRepository.GetById(value.PatientId);
                if (patient == null)
                {
                    return NotFound($"Patient with id {value.PatientId} wasn't found.");
                }

                var disease = _diseaseRepository.GetById(value.DiseaseId);
                if (disease == null)
                {
                    return NotFound($"Disease with id {value.PatientId} wasn't found.");
                }

                var existingMedicalHistory = _repository.GetExistingMedicalDocument(value.PatientId, value.DiseaseId);
                if (existingMedicalHistory != null)
                {
                    return Conflict("Such medical history already exists.");
                }

                var newMedicalHistory = _mapper.Map<MedicalHistory>(value);
                newMedicalHistory.Patient = patient;
                newMedicalHistory.Disease = disease;
                _repository.Insert(newMedicalHistory);
                _repository.Save();

                value.Id = newMedicalHistory.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<MedicalHistoryDTO> Put(int id, [FromBody] MedicalHistoryDTO value)
        {
            var existingMedicalHistory = _repository.GetById(id);

            if (existingMedicalHistory == null)
            {
                return NotFound($"Medical history with id {id} wasn't found.");
            }

            existingMedicalHistory.DiseaseId = value.DiseaseId;
            existingMedicalHistory.StartDate = DateOnly.FromDateTime(value.StartDate);
            existingMedicalHistory.EndDate = value.EndDate.HasValue ? DateOnly.FromDateTime(value.EndDate.Value) : null;

            _repository.Update(existingMedicalHistory);
            _repository.Save();

            value.Id = existingMedicalHistory.Id;

            return Ok(value);
        }

        [HttpDelete("{id}")]
        public ActionResult<MedicalHistoryDTO> Delete(int id)
        {
            try
            {
                var deletedMedicalHistory = _repository.Delete(id);
                if (deletedMedicalHistory == null)
                {
                    return NotFound($"Medical history with id {id} wasn't found.");
                }

                var deletedMedicalHistoryDto = _mapper.Map<MedicalHistoryDTO>(deletedMedicalHistory);

                return Ok(deletedMedicalHistoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
