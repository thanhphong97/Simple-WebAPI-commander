using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Models;
using Commander.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandFromRepo = _repository.GetCommandById(id);

            if (commandFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(commandFromRepo));
        }

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommand()
        {
            var commandFromRepo = _repository.GetAllCommand();

            if (commandFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandFromRepo));
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateComnad(CommandCreateDto commandCreateDto)
        {
            var commandCreate = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandCreate);

            var commandReadDto = _mapper.Map<CommandReadDto>(commandCreate);
            return CreatedAtRoute(nameof(GetCommandById), new { id = commandReadDto.Id }, commandReadDto);
            // return Ok(commandCreate);
        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult<CommandReadDto> UpdateCommnad(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandFromRepo = _repository.GetCommandById(id);

            if (commandFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandFromRepo);
            _repository.UpdateCommand(commandFromRepo);

            var commandReadDto = _mapper.Map<CommandReadDto>(commandFromRepo);
            return Ok(commandReadDto);
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult UpdatePatialCommand(int id, JsonPatchDocument<CommandUpdateDto> jsonDoc)
        {
            var commandFromRepo = _repository.GetCommandById(id);

            if (commandFromRepo == null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandFromRepo);
            jsonDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandFromRepo);

            _repository.UpdateCommand(commandFromRepo);

            return NoContent();
        }
    }
}