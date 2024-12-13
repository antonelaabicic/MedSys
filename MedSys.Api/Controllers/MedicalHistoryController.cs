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
        public ActionResult<MedicalHistoryDTO> Post([FromBody] MedicalHistoryDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var patient = _patientRepository.FindPatientByDetails(value.Patient.FirstName, value.Patient.LastName, value.Patient.DateOfBirth);
                if (patient == null)
                {
                    patient = _mapper.Map<Patient>(value.Patient);
                    _patientRepository.Insert(patient);
                    _patientRepository.Save();
                    value.Patient.Id = patient.Id;
                }

                var disease = _diseaseRepository.GetAll().FirstOrDefault(d => d.Name == value.Disease.Name);
                if (disease == null)
                {
                    disease = _mapper.Map<Disease>(value.Disease);
                    _diseaseRepository.Insert(disease);
                    _diseaseRepository.Save();
                    value.Disease.Id = disease.Id;
                }

                var existingMedicalHistory = _repository.GetExistingMedicalDocument(value.Patient.Id, value.Disease.Id);
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

            existingMedicalHistory.Patient.FirstName = value.Patient.FirstName ?? existingMedicalHistory.Patient.FirstName;
            existingMedicalHistory.Patient.LastName = value.Patient.LastName ?? existingMedicalHistory.Patient.LastName;
            existingMedicalHistory.Patient.DateOfBirth = DateOnly.FromDateTime(value.Patient.DateOfBirth);
            existingMedicalHistory.Patient.Gender = value.Patient.Gender ?? existingMedicalHistory.Patient.Gender;
            existingMedicalHistory.Patient.Oib = value.Patient.Oib ?? existingMedicalHistory.Patient.Oib;
            existingMedicalHistory.Disease.Name = value.Disease.Name ?? existingMedicalHistory.Disease.Name;
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
