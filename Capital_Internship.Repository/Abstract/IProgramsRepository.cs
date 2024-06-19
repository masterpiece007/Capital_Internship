using Capital_Internship.Domain.Dtos;
using Capital_Internship.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Repository.Abstract
{
    public interface IProgramsReository
    {
        Task<bool> CreateProgramAndApplication(Program_ product);
        Task<Program_> GetFullProgramDetails(Guid programId);
        Task<List<GetProgramDto>> GetAllPrograms();
        Task<bool> EditProgramQuestion(Guid additionalQuestionId, EditAdditionalQuestionDto dto);
        Task<bool> SubmitCandidateApplication(CandidateApplication model);
        Task<List<CandidateApplication>> GetCandidateApplicationsByProgramId(Guid programId);
    }
}
