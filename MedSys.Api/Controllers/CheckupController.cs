using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckupController : ControllerBase
    {
        private readonly ICheckupRepository _repository;
        private readonly ICheckupTypeRepository _checkupTypeRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public CheckupController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<ICheckupRepository>();
            _checkupTypeRepository = repositoryFactory.GetRepository<ICheckupTypeRepository>();
            _patientRepository = repositoryFactory.GetRepository<IPatientRepository>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CheckupDTO>> Get()
        {
            try
            {
                var checkups = _repository.GetAll();
                var checkupDtos = _mapper.Map<IEnumerable<CheckupDTO>>(checkups);
                return Ok(checkupDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CheckupDTO> Get(int id)
        {
            try
            {
                var checkup = _repository.GetById(id);
                if (checkup == null)
                {
                    return NotFound($"Checkup with id {id} wasn't found.");
                }
                var result = _mapper.Map<CheckupDTO>(checkup);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<CheckupDTO> Post([FromBody] CheckupSimplifiedDTO value)
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

                var checkupType = _checkupTypeRepository.GetById(value.CheckupTypeId);
                if (checkupType == null)
                {
                    return NotFound($"Such checkup type does not exist.");
                }

                var existingCheckup = _repository.GetExistingCheckup(value.PatientId, value.CheckupTypeId, DateOnly.FromDateTime(value.CheckupDateTime));
                if (existingCheckup != null)
                {
                    return Conflict("Such checkup already exists.");
                }

                var newCheckup = _mapper.Map<Checkup>(value);
                newCheckup.CheckupType = checkupType;
                newCheckup.Patient = patient;
                _repository.Insert(newCheckup);
                _repository.Save();

                value.Id = newCheckup.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<CheckupDTO> Put(int id, [FromBody] CheckupDTO value)
        {
            try
            {
                var existingCheckup = _repository.GetById(id);

                if (existingCheckup == null)
                {
                    return NotFound($"Checkup with id {id} wasn't found.");
                }

                existingCheckup.CheckupTypeId = value.CheckupTypeId;
                existingCheckup.Date = DateOnly.FromDateTime(value.CheckupDateTime);
                existingCheckup.Time = TimeOnly.FromDateTime(value.CheckupDateTime);

                _repository.Update(existingCheckup);
                _repository.Save();

                value.Id = existingCheckup.Id;

                return Ok(value); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("patient/{patientId}")]
        public ActionResult<IEnumerable<CheckupDTO>> GetCheckupsByPatient(int patientId)
        {
            try
            {
                var checkups = _repository.GetCheckupsByPatientId(patientId);
                if (!checkups.Any())
                {
                    return NotFound($"No checkups found for patient with id {patientId}.");
                }

                var checkupDTOs = checkups.Select(c => _mapper.Map<CheckupDTO>(c)).ToList(); 
                return Ok(checkupDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<CheckupDTO> Delete(int id)
        {
            try
            {
                var deletedCheckup = _repository.Delete(id);
                if (deletedCheckup == null)
                {
                    return NotFound($"Checkup with id {id} wasn't found.");
                }

                var deletedCheckupDto = _mapper.Map<CheckupDTO>(deletedCheckup);

                return Ok(deletedCheckupDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
