using Capital_Internship.Domain.Dtos;
using Capital_Internship.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Capital_Internship.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramsService _programsService;

        public ProgramsController(IProgramsService programsService)
        {
            _programsService = programsService;
        }

        [HttpPost("CreateProgramAndApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProgramAndApplication(ProgramDto model)
        {
            var result = await _programsService.CreateProgramAndApplication(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("SubmitCandidateApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitCandidateApplication(CandidateApplicationDto model)
        {
            var result = await _programsService.SubmitCandidateApplication(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetFullProgramDetails/{programId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFullProgramDetails(Guid programId)
        {
            var result = await _programsService.GetProgramById(programId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetCandidateApplicationsByProgramId/{programId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCandidateApplicationsByProgramId(Guid programId)
        {
            var result = await _programsService.GetCandidateApplicationsByProgramId(programId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpGet("GetAllPrograms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPrograms()
        {
            var result = await _programsService.GetAllPrograms();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("EditProgramQuestion/{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditProgramQuestion(Guid questionId, EditAdditionalQuestionDto dto)
        {
            var result = await _programsService.EditProgramQuestion(questionId, dto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
