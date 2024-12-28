using AutoMapper;
using MedSys.Api.Dtos;
using MedSys.Api.DTOs;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly IRepository<Disease> _repository;
        private readonly IMapper _mapper;

        public DiseaseController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IRepository<Disease>>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DiseaseDTO>> Get()
        {
            try
            {
                var diseases = _repository.GetAll();
                var diseasesDtos = _mapper.Map<IEnumerable<DiseaseDTO>>(diseases);
                return Ok(diseasesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DiseaseDTO> Get(int id)
        {
            var disease = _repository.GetById(id);
            if (disease == null)
            {
                return NotFound($"Disease with id {id} was not found.");
            }

            var dto = _mapper.Map<DiseaseDTO>(disease);

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<DiseaseDTO> Post([FromBody] DiseaseDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDisease = _repository.GetAll().FirstOrDefault(b => b.Name == value.Name);
                if (existingDisease != null)
                {
                    return StatusCode(409, $"Disease of such name already exists.");
                }

                var newDisease = _mapper.Map<Disease>(value);
                _repository.Insert(newDisease);
                _repository.Save();

                value.Id = newDisease.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<DiseaseDTO> Put(int id, [FromBody] DiseaseDTO value)
        {
            try
            {
                var existingDisease = _repository.GetById(id);
                if (existingDisease == null)
                {
                    return NotFound($"Disease with id {id} wasn't found.");
                }

                existingDisease.Name = value.Name ?? existingDisease.Name;

                _repository.Update(existingDisease);
                _repository.Save();

                value.Id = existingDisease.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<DiseaseDTO> Delete(int id)
        {
            try
            {
                var deletedDisease = _repository.Delete(id);

                if (deletedDisease == null)
                {
                    return NotFound($"Disease with id {id} wasn't found.");
                }

                var deletedDiseaseDto = _mapper.Map<DiseaseDTO>(deletedDisease);

                return Ok(deletedDiseaseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
