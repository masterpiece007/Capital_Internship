using Capital_Internship.Domain.Dtos;
using Capital_Internship.Domain.Helpers;
using Capital_Internship.Domain.Models;
using Capital_Internship.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Repository.Concrete
{
    public class ProgramsReository : IProgramsReository
    {
        private readonly AppDbContext _dbContext;

        public ProgramsReository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateProgramAndApplication(Program_ model)
        {
            try
            {
                _dbContext.Add(model);
                var rowsInserted = await _dbContext.SaveChangesAsync();
                return rowsInserted >= 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> SubmitCandidateApplication(CandidateApplication model)
        {
            try
            {
                _dbContext.Add(model);
                var rowsInserted = await _dbContext.SaveChangesAsync();
                return rowsInserted >= 1;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in repo-function SubmitCandidateApplication: message {e.Message},inner-exception: {e.InnerException?.Message} stacktrace: {e.StackTrace}");
                return false;
            }

        }

        public async Task<List<GetProgramDto>> GetAllPrograms()
        {
            try
            {
                var program = await _dbContext.Programs.Select(a => new GetProgramDto { Id = a.Id, Title = a.Title, Description = a.Description, DateCreated = a.DateCreated }).ToListAsync();
                return program;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in repo-function GetAllPrograms: message {e.Message},inner-exception: {e.InnerException?.Message} stacktrace: {e.StackTrace}");
                return null;
            }

        }

        public async Task<Program_> GetFullProgramDetails(Guid programId)
        {
            try
            {
                var program = await _dbContext.Programs.FindAsync(programId);
                if (program == null)
                    return null;

                var programEntry = _dbContext.Programs.Entry(program);
                await programEntry.Reference(x => x.ApplicationRequirement).LoadAsync();

                var appReqId = program.ApplicationRequirement.Id;
                var appRequirement = await _dbContext.ApplicationRequirements.FindAsync(appReqId);
                if (appRequirement == null)
                    return program;

                var appRequirementEntry = _dbContext.ApplicationRequirements.Entry(appRequirement);
                await appRequirementEntry.Collection(x => x.AdditionalQuestions).LoadAsync();

                program.ApplicationRequirement.AdditionalQuestions.ForEach(q =>
                {
                    if (q.ReplyFormat == ReplyFormat.MultipleChoice || q.ReplyFormat == ReplyFormat.Dropdown)
                    {
                        var choices = _dbContext.QuestionChoices.Where(a => a.AdditionalQuestionId == q.Id).Distinct().AsEnumerable();
                        if (choices.ToList().Count > 0)
                        {
                            //q.QuestionChoices.AddRange(choices);
                        }
                    };
                });
                return program;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in repo-function SubmitCandidateApplication: message {e.Message},inner-exception: {e.InnerException?.Message} stacktrace: {e.StackTrace}");
                return null;
            }

        }
        public async Task<List<CandidateApplication>> GetCandidateApplicationsByProgramId(Guid programId)
        {
            try
            {
                var program = await _dbContext.Programs.FindAsync(programId);
                if (program == null)
                    return null;

                var programEntry = _dbContext.Programs.Entry(program);
                await programEntry.Collection(x => x.CandidateApplications).LoadAsync();

                program.CandidateApplications.ForEach(ca =>
                {
                    var questionResponses = _dbContext.QuestionResponses.Where(a => a.CandidateApplicationId == ca.Id).Distinct().AsEnumerable();
                    if (questionResponses.ToList().Count > 0)
                    {
                        //q.QuestionChoices.AddRange(choices);
                        questionResponses.ToList().ForEach(questionResponse =>
                        {
                            if (questionResponse.ExpectedReplyFormat == ReplyFormat.MultipleChoice)
                            {
                                var selectedItems = _dbContext.SelectedDropdownItems.Where(a => a.QuestionResponseId == questionResponse.Id).ToList();
                                questionResponse.SelectedDropdownItems = selectedItems;
                            }
                        });
                    }
                });
                return program.CandidateApplications;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in repo-function GetCandidateApplicationsByProgramId: message {e.Message},inner-exception: {e.InnerException?.Message} stacktrace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<bool> EditProgramQuestion(Guid additionalQuestionId, EditAdditionalQuestionDto dto)
        {
            try
            {
                var additionalQuestionToUpdate = await _dbContext.AdditionalQuestions.FindAsync(additionalQuestionId);
                if (additionalQuestionToUpdate == null)
                    return false;

                var additionalQuestionEntry = _dbContext.AdditionalQuestions.Entry(additionalQuestionToUpdate);
                await additionalQuestionEntry.Collection(x => x.QuestionChoices).LoadAsync();
                if (dto.ReplyFormat == ReplyFormat.MultipleChoice)
                {
                    if (additionalQuestionToUpdate.QuestionChoices.Count > 0)
                    {
                        //clear 
                        var existingQuestionChoices = additionalQuestionToUpdate.QuestionChoices;
                        _dbContext.QuestionChoices.RemoveRange(existingQuestionChoices);

                        //create new entries
                        var listOfQuestionChoices = dto.QuestionChoices.Select(a => new QuestionChoice { AdditionalQuestionId = additionalQuestionId, Value = a.Value });
                        _dbContext.AddRange(listOfQuestionChoices);
                    }
                }
                additionalQuestionToUpdate.QuestionBody = dto.QuestionBody;
                additionalQuestionToUpdate.ReplyFormat = dto.ReplyFormat;
                additionalQuestionToUpdate.MaxNoOfChoiceAllowed = dto.MaxNoOfChoiceAllowed;
                additionalQuestionToUpdate.DateModified = DateTime.UtcNow;

                var rowsModified = await _dbContext.SaveChangesAsync();
                if (rowsModified <= 0)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured in repo-function EditProgramQuestion: message {e.Message},inner-exception: {e.InnerException?.Message} stacktrace: {e.StackTrace}");
                return false;
            }
        }
    }

}
