using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDrugRepository _drugRepository;
        private readonly IMapper _mapper;
        public PrescriptionController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IPrescriptionRepository>();
            _patientRepository = repositoryFactory.GetRepository<IPatientRepository>();
            _drugRepository = repositoryFactory.GetRepository<IDrugRepository>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PrescriptionDTO>> Get()
        {
            try
            {
                var prescriptions = _repository.GetAll();
                var prescriptionsDtos = _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
                return Ok(prescriptionsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PrescriptionDTO> Get(int id)
        {
            var prescription = _repository.GetById(id);
            if (prescription == null)
            {
                return NotFound($"Prescription with id {id} was not found.");
            }

            var dto = _mapper.Map<PrescriptionDTO>(prescription);

            return Ok(dto);
        }

        [HttpGet("patient/{patientId}")]
        public ActionResult<IEnumerable<PrescriptionDTO>> GetByPatientId(int patientId)
        {
            var prescriptions = _repository.GetPrescriptionByPatientId(patientId);
            if (!prescriptions.Any())
            {
                return NotFound($"No prescriptions found for patient with id {patientId}.");
            }

            var dtos = prescriptions.Select(p => _mapper.Map<PrescriptionDTO>(p)).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<PrescriptionDTO> Post([FromBody] PrescriptionSimplifiedDTO value)
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

                var drug = _drugRepository.GetById(value.DrugId);
                if (drug == null)
                {
                    return NotFound($"Drug with id {value.DrugId} wasn't found.");
                }

                var existingPrescription = _repository.GetExistingPrescription(value.PatientId, value.DrugId, value.IssueDate);
                if (existingPrescription != null)
                {
                    return Conflict("Such prescription already exists.");
                }

                var newPrescription = _mapper.Map<Prescription>(value);
                newPrescription.Patient = patient;
                newPrescription.Drug = drug;
                _repository.Insert(newPrescription);
                _repository.Save();

                value.Id = newPrescription.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<PrescriptionDTO> Put(int id, [FromBody] PrescriptionDTO value)
        {
            var existingPrescription = _repository.GetById(id);

            if (existingPrescription == null)
            {
                return NotFound($"Prescription with id {id} wasn't found.");
            }

            existingPrescription.DrugId = value.DrugId;
            existingPrescription.Frequency = value.Frequency ?? existingPrescription.Frequency;
            existingPrescription.Duration = value.Duration ?? existingPrescription.Duration;
            existingPrescription.Dosage = value.Dosage ?? existingPrescription.Dosage;
            existingPrescription.IssueDate = DateOnly.FromDateTime(value.IssueDate);

            _repository.Update(existingPrescription);
            _repository.Save();

            value.Id = existingPrescription.Id;

            return Ok(value);
        }

        [HttpDelete("{id}")]
        public ActionResult<PrescriptionDTO> Delete(int id)
        {
            try
            {
                var deletedPrescription = _repository.Delete(id);
                if (deletedPrescription == null)
                {
                    return NotFound($"Prescription with id {id} wasn't found.");
                }

                var deletedPrescriptionDto = _mapper.Map<PrescriptionDTO>(deletedPrescription);

                return Ok(deletedPrescriptionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
