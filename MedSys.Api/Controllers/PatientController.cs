using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _repository;
        private readonly ICheckupRepository _checkupRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;
        private readonly IMapper _mapper;
        public PatientController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IPatientRepository>();
            _checkupRepository = repositoryFactory.GetRepository<ICheckupRepository>();
            _prescriptionRepository = repositoryFactory.GetRepository<IPrescriptionRepository>();
            _medicalHistoryRepository = repositoryFactory.GetRepository<IMedicalHistoryRepository>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientDTO>> Get()
        {
            try
            {
                var patients = _repository.GetAll();
                var patientDtos = _mapper.Map<IEnumerable<PatientDTO>>(patients);
                return Ok(patientDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PatientDTO> Get(int id)
        {
            var patient = _repository.GetById(id);
            if (patient == null)
            {
                return NotFound($"Patient with id {id} was not found.");
            }

            var dto = _mapper.Map<PatientDTO>(patient);
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<PatientDTO> Post([FromBody] PatientDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingPatient = _repository.FindPatientByDetails(value.FirstName, value.LastName, value.DateOfBirth);
                if (existingPatient != null)
                {
                    return StatusCode(409, $"Such patient already exists.");
                }

                var newPatient = _mapper.Map<Patient>(value);
                _repository.Insert(newPatient);
                _repository.Save();

                value.Id = newPatient.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<PatientDTO> Put(int id, [FromBody] PatientDTO value)
        {
            try
            {
                var existingPatient = _repository.GetById(id);
                if (existingPatient == null)
                {
                    return NotFound($"Patient with id {id} wasn't found.");
                }

                existingPatient.FirstName = value.FirstName ?? existingPatient.FirstName;
                existingPatient.LastName = value.LastName ?? existingPatient.LastName;
                existingPatient.DateOfBirth = DateOnly.FromDateTime(value.DateOfBirth); ;
                existingPatient.Gender = value.Gender ?? existingPatient.Gender;
                existingPatient.Oib = value.Oib ?? existingPatient.Oib;

                _repository.Update(existingPatient);
                _repository.Save();

                value.Id = existingPatient.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<PatientDTO> Delete(int id)
        {
            try
            {
                var deletedPatient = _repository.Delete(id);
                if (deletedPatient == null)
                {
                    return NotFound($"Patient with id {id} wasn't found.");
                }

                var deletedPatientDto = _mapper.Map<PatientDTO>(deletedPatient);
                return Ok(deletedPatientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PatientDTO>> Search([FromQuery] string? lastName, [FromQuery] string? oib)
        {
            try
            {
                var patients = _repository.GetAll();

                if (!string.IsNullOrEmpty(lastName))
                {
                    patients = patients.Where(p => p.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(oib))
                {
                    patients = patients.Where(p => p.Oib.Contains(oib, StringComparison.OrdinalIgnoreCase));
                }

                var patientDtos = _mapper.Map<IEnumerable<PatientDTO>>(patients);
                return Ok(patientDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("export")]
        public IActionResult ExportToCsv()
        {
            string SEPARATOR = ";";
            try
            {
                var patients = _repository.GetAll().OrderBy(p => p.Id);
                var csvBuilder = new StringBuilder();

                foreach (var patient in patients)
                {
                    csvBuilder.AppendLine($"Id{SEPARATOR}FirstName{SEPARATOR}LastName{SEPARATOR}DOB{SEPARATOR}Gender{SEPARATOR}OIB");
                    csvBuilder.AppendLine(
                        $"{patient.Id}{SEPARATOR}{patient.FirstName}{SEPARATOR}{patient.LastName}{SEPARATOR}{patient.DateOfBirth:yyyy-MM-dd}{SEPARATOR}{patient.Gender}{SEPARATOR}{patient.Oib}"
                    );

                    var checkups = _checkupRepository.GetCheckupsByPatientId(patient.Id);
                    var prescriptions = _prescriptionRepository.GetPrescriptionByPatientId(patient.Id);
                    var medicalHistories = _medicalHistoryRepository.GetMedicalHistoryByPatientId(patient.Id);

                    if (checkups.Any())
                    {
                        csvBuilder.AppendLine("---------------------------------------");
                        csvBuilder.AppendLine($"CheckupId{SEPARATOR}CheckupType{SEPARATOR}Date{SEPARATOR}Time");
                        foreach (var checkup in checkups)
                        {
                            csvBuilder.AppendLine(
                                $"{checkup.Id}{SEPARATOR}{checkup.CheckupType.Name}{SEPARATOR}{checkup.Date:dd.MM.yyyy}{SEPARATOR}{checkup.Time:HH:mm}"
                            );
                        }
                    }

                    if (prescriptions.Any())
                    {
                        csvBuilder.AppendLine("---------------------------------------");
                        csvBuilder.AppendLine($"PrescriptionId{SEPARATOR}Drug{SEPARATOR}Dosage{SEPARATOR}Frequency{SEPARATOR}Duration{SEPARATOR}IssueDate");
                        foreach (var prescription in prescriptions)
                        {
                            csvBuilder.AppendLine(
                                $"{prescription.Id}{SEPARATOR}{prescription.Drug.Name}{SEPARATOR}{prescription.Dosage}{SEPARATOR}{prescription.Frequency}{SEPARATOR}{prescription.Duration}{SEPARATOR}{prescription.IssueDate:dd.MM.yyyy}"
                            );
                        }
                    }

                    if (medicalHistories.Any())
                    {
                        csvBuilder.AppendLine("---------------------------------------");
                        csvBuilder.AppendLine($"MedicalHistoryId{SEPARATOR}Disease{SEPARATOR}StartDate{SEPARATOR}EndDate");
                        foreach (var history in medicalHistories)
                        {
                            csvBuilder.AppendLine(
                                $"{history.Id}{SEPARATOR}{history.Disease.Name}{SEPARATOR}{history.StartDate:dd.MM.yyyy}{SEPARATOR}{history.EndDate:dd.MM.yyyy}"
                            );
                        }
                    }
                    csvBuilder.AppendLine().AppendLine();
                }

                var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                var fileName = "Patients.csv";

                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error exporting data: {ex.Message}");
            }
        }


    }
}
