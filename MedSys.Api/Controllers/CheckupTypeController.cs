using AutoMapper;
using MedSys.Api.DTOs;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MedSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckupTypeController : ControllerBase
    {
        private readonly ICheckupTypeRepository _repository;
        private readonly IMapper _mapper;

        public CheckupTypeController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<ICheckupTypeRepository>();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CheckupTypeDTO>> Get()
        {
            try
            {
                var checkupTypes = _repository.GetAll();
                var checkupTypesDtos = _mapper.Map<IEnumerable<CheckupTypeDTO>>(checkupTypes);
                return Ok(checkupTypesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<CheckupTypeDTO> Post([FromBody] CheckupTypeDTO value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCheckupType = _repository.GetAll().FirstOrDefault(b => b.Name == value.Name);
                if (existingCheckupType != null)
                {
                    return StatusCode(409, $"Checkup type of such name already exists.");
                }

                var newCheckupType = _mapper.Map<CheckupType>(value);
                _repository.Insert(newCheckupType);
                _repository.Save();

                value.Id = newCheckupType.Id;

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<CheckupTypeDTO> Put(int id, [FromBody] CheckupTypeDTO value)
        {
            try
            {
                var existingCheckupType = _repository.GetById(id);
                if (existingCheckupType == null)
                {
                    return NotFound($"Checkup type with id {id} wasn't found.");
                }

                existingCheckupType.Name = value.Name ?? existingCheckupType.Name;
                existingCheckupType.Code = value.Code ?? existingCheckupType.Code;

                _repository.Update(existingCheckupType);
                _repository.Save();

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<CheckupTypeDTO> Delete(int id)
        {
            try
            {
                var deletedCheckupType = _repository.Delete(id);

                if (deletedCheckupType == null)
                {
                    return NotFound($"Checkup type with id {id} wasn't found.");
                }

                var deletedCheckupTypeDto = _mapper.Map<CheckupTypeDTO>(deletedCheckupType);

                return Ok(deletedCheckupTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
