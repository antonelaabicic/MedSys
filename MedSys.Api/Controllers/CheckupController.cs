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
    public class CheckupController : ControllerBase
    {
        private readonly IRepository<Checkup> _repository;
        private readonly IMapper _mapper;

        public CheckupController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repository = repositoryFactory.GetRepository<Checkup>();
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

        // GET api/<CheckupController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CheckupController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CheckupController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CheckupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
