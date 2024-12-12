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
    public class DrugController : ControllerBase
    {
        private readonly IRepository<Drug> _repository;
        private readonly IMapper _mapper;

        public DrugController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<IRepository<Drug>>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DrugDTO>> Get()
        {
            try
            {
                var drugs = _repository.GetAll();
                var drugsDtos = _mapper.Map<IEnumerable<DrugDTO>>(drugs);
                return Ok(drugsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<DrugDTO> Post([FromBody] DrugDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDrug = _repository.GetAll().FirstOrDefault(b => b.Name == value.Name);
                if (existingDrug != null)
                {
                    return StatusCode(409, $"Drug of such name already exists.");
                }

                var newDrug = _mapper.Map<Drug>(value);
                _repository.Insert(newDrug);
                _repository.Save();

                value.Id = newDrug.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<DrugDTO> Put(int id, [FromBody] DrugDTO value)
        {
            try
            {
                var existingDrug = _repository.GetById(id);
                if (existingDrug == null)
                {
                    return NotFound($"Drug with id {id} wasn't found.");
                }

                existingDrug.Name = value.Name ?? existingDrug.Name;

                _repository.Update(existingDrug);
                _repository.Save();

                value.Id = existingDrug.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<DrugDTO> Delete(int id)
        {
            try
            {
                var deletedDrug = _repository.Delete(id);

                if (deletedDrug == null)
                {
                    return NotFound($"Drug with id {id} wasn't found.");
                }

                var deletedDrugDto = _mapper.Map<DrugDTO>(deletedDrug);

                return Ok(deletedDrugDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
