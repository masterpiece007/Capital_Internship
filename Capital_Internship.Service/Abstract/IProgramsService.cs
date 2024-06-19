using Capital_Internship.Domain.Dtos;
using Capital_Internship.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Service.Abstract
{
    public interface IProgramsService
    {
        Task<GenericResponse> CreateProgramAndApplication(ProgramDto dto);
        Task<GenericResponse<List<string>>> SubmitCandidateApplication(CandidateApplicationDto dto);
        Task<GenericResponse> EditProgramQuestion(Guid AdditionalQuestionId, EditAdditionalQuestionDto dto);
        Task<GenericResponse<List<GetProgramDto>>> GetAllPrograms();
        Task<GenericResponse> GetProgramById(Guid programId);
        Task<GenericResponse> GetCandidateApplicationsByProgramId(Guid programId);
    }
}
