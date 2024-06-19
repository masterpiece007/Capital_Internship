using Capital_Internship.Domain.Dtos;
using Capital_Internship.Domain.Helpers;
using Capital_Internship.Domain.Models;
using Capital_Internship.Repository.Abstract;
using Capital_Internship.Service.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Service.Concrete
{
    public class ProgramsService : IProgramsService
    {
        private readonly IProgramsRepository _programsRepository;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public string cacheKey = "ProgramsList";
        private int cacheExpiryInSeconds;

        public ProgramsService(IProgramsRepository programsRepository, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _programsRepository = programsRepository;
            _configuration = configuration;
            _memoryCache = memoryCache;
            cacheExpiryInSeconds = _configuration.GetValue<int>("CacheExpiryInSeconds");

        }
        public async Task<GenericResponse> CreateProgramAndApplication(ProgramDto dto)
        {
            try
            {
                //Log.Information("entering CreateProgramAndApplication method.");

                if (dto == null)
                {
                    //Log.Information("invalid program values provided in CreateProgramAndApplication");
                    return new GenericResponse().Failure("invalid program values provided");
                }

                var program = dto.MapToProgram_();
                program.ApplicationRequirement = dto.AppRequirement.MapToApplicationRequirement();
                var newAdditionalQuestions = program.ApplicationRequirement.AdditionalQuestions;

                var additionalQuestionsProvided = dto.AppRequirement.AdditionalQuestions;
                if (additionalQuestionsProvided.Count > 0)
                    additionalQuestionsProvided.ForEach(q => newAdditionalQuestions.Add(q.MapToAdditionalQuestion()));

                var isSaved = await _programsRepository.CreateProgramAndApplication(program);
                if (isSaved)
                {
                    //Log.Information($"removing programs-list from cache due to creation of new program");
                    _memoryCache.Remove(cacheKey);

                    //Log.Information($"Program added successfully. serialized-payload: {JsonConvert.SerializeObject(dto)}");
                    return new GenericResponse().Successful("Program added successfully");
                }

                return new GenericResponse().Failure("Failed create program");
            }
            catch (Exception e)
            {
                //todo: add logs
                //Log.Error($"An error occured in CreateProgramAndApplication: {e.Message}, stacktrace: {e.StackTrace}");
                return new GenericResponse().Failure("An error occured, kindly try again");
            }
        }

        public async Task<GenericResponse> EditProgramQuestion(Guid additionalQuestionId, EditAdditionalQuestionDto dto)
        {
            //Log.Information($"entering EditProgramQuestion method. additionalQuestionId: {additionalQuestionId}");

            var response = new GenericResponse();
            try
            {
                if (dto == null || additionalQuestionId == null)
                {
                    //Log.Information("invalid data provided");
                    return response.Failure("invalid data provided");
                }

                var result = await _programsRepository.EditProgramQuestion(additionalQuestionId, dto);
                if (result)
                {
                    //Log.Information($"removing key: {dto.ProgramId} from cache due to Edit");
                    _memoryCache.Remove(dto.ProgramId);
                    response.Data = dto;

                    //Log.Information($"Question details updated successfully. additionalQuestionId: {additionalQuestionId}");
                    return response.Successful("Question details updated successfully");
                }

                response.Data = dto;
                //Log.Information($"Unable to update Question details. additionalQuestionId: {additionalQuestionId}");
                return response.Failure("Unable to update Question details");
            }
            catch (Exception e)
            {
                //Log.Error($"An error occured in EditProgramQuestion: {e.Message}, stacktrace: {e.StackTrace}");
                return response.Failure("An error occured, kindly try again");
            }
        }

        public async Task<GenericResponse<List<GetProgramDto>>> GetAllPrograms()
        {
            try
            {
                //Log.Information($"entering GetAllPrograms method.");

                var cachedProgramsList = new List<GetProgramDto>();
                if (!_memoryCache.TryGetValue(cacheKey, out cachedProgramsList))
                {
                    cachedProgramsList = await _programsRepository.GetAllPrograms();

                    _memoryCache.Set(cacheKey, cachedProgramsList, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheExpiryInSeconds)));
                }

                return new GenericResponse<List<GetProgramDto>>() { Data = cachedProgramsList, Success = true };
            }
            catch (Exception e)
            {
                //Log.Error($"An error occured in GetProgramById: {e.Message},inner-exception: {e.InnerException?.Message}, stacktrace: {e.StackTrace}");
                return new GenericResponse<List<GetProgramDto>>().Failure("An error occured, kindly try again");
            }
        }

        public async Task<GenericResponse> GetProgramById(Guid programId)
        {
            try
            {
                //Log.Information($"entering GetProgramById method. programId: {programId}");

                var response = new GenericResponse();

                var cachedProgramDetail = new Program_();
                if (!_memoryCache.TryGetValue(programId, out cachedProgramDetail))
                {
                    var result = await _programsRepository.GetFullProgramDetails(programId);
                    if (result == null)
                        return new GenericResponse().Failure("No Program was found.");

                    response.Success = true;
                    response.Data = cachedProgramDetail = result;

                    _memoryCache.Set(programId, cachedProgramDetail, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheExpiryInSeconds)));
                }
                return new GenericResponse() { Data = cachedProgramDetail, Success = true };
            }
            catch (Exception e)
            {
                //Log.Error($"An error occured in GetProgramById: {e.Message},inner-exception: {e.InnerException?.Message}, stacktrace: {e.StackTrace}");
                return new GenericResponse().Failure("An error occured, kindly try again");
            }
        }
        public async Task<GenericResponse> GetCandidateApplicationsByProgramId(Guid programId)
        {
            var response = new GenericResponse();
            try
            {
                var result = await _programsRepository.GetCandidateApplicationsByProgramId(programId);
                if (result == null)
                    return new GenericResponse().Failure("No Application was submitted for this Program.");

                response.Success = true;
                response.Data = result;
                return response;
            }
            catch (Exception e)
            {
                //Log.Error($"An error occured in GetCandidateApplicationsByProgramId: {e.Message},inner-exception: {e.InnerException?.Message}, stacktrace: {e.StackTrace}");
                return response.Failure("An error occured, kindly try again");
            }

        }
        public async Task<GenericResponse<List<string>>> SubmitCandidateApplication(CandidateApplicationDto dto)
        {
            //Log.Information($"entering SubmitCandidateApplication method. serialized-payload: {JsonConvert.SerializeObject(dto)}");
            var response = new GenericResponse<List<string>>();
            try
            {
                if (dto == null)
                {
                    //Log.Information("invalid data provided");
                    return response.Failure("invalid data provided");
                }

                //Validate Answer Format
                var invalidAnswers = new List<string>();

                if (dto.QuestionResponses.Count > 0)
                {
                    foreach (var item in dto.QuestionResponses)
                    {
                        var isValid = ValidateAnswerFormat(item.ExpectedReplyFormat, item.CandidateAnswer, item.MaxNoOfChoiceAllowed, item.SelectedDropdownItems);
                        if (!isValid)
                        {
                            if (item.ExpectedReplyFormat == ReplyFormat.MultipleChoice)
                            {
                                var flattenedString = String.Join(',', item.SelectedDropdownItems.Select(a => a.Value));
                                invalidAnswers.Add($"{flattenedString} exceed the number of items you can pick({item.MaxNoOfChoiceAllowed})");
                            }
                            else
                            {
                                invalidAnswers.Add($"'{item.CandidateAnswer}' is invalid, expected response should be in {Enum.GetName(typeof(ReplyFormat),item.ExpectedReplyFormat)} format");
                            };
                        }
                    }
                    if (invalidAnswers.Count > 0)
                    {
                        response.Data = invalidAnswers;
                        return response.Failure("invalid response provided");
                    }
                }

                //mapping
                var candidateApplication = dto.MapToCandidateApplication();
                if (dto.QuestionResponses.Count > 0)
                    dto.QuestionResponses.ForEach(item => { candidateApplication.QuestionResponses.Add(item.MapToQuestionResponse()); });

                var isSaved = await _programsRepository.SubmitCandidateApplication(candidateApplication);
                if (isSaved)
                {
                    //Log.Information($"Application submitted successfully.  serialized-payload: {JsonConvert.SerializeObject(dto)}");
                    return response.Successful("Application submitted successfully");
                }
                //Log.Information($"Failed to submit application.  serialized-payload: {JsonConvert.SerializeObject(dto)}");
                return response.Failure("Failed to submit application");
            }
            catch (Exception e)
            {
                //Log.Error($"An error occured in SubmitCandidateApplication: {e.Message}, serialized-payload: {JsonConvert.SerializeObject(dto)}, stacktrace: {e.StackTrace}");
                return response.Failure("An error occured, kindly try again");
            }

        }

        public bool ValidateAnswerFormat(ReplyFormat expectedReplyFormat, string candidateAnswer, int maxNoOfItemsToSelect, List<SelectedDropdownItemDto> selectedItems = null)
        {
            switch (expectedReplyFormat)
            {
                case ReplyFormat.Date:
                    return DateTime.TryParse(candidateAnswer, out _) || DateOnly.TryParse(candidateAnswer, out _);

                case ReplyFormat.YesOrNo:
                    return bool.TryParse(candidateAnswer, out _);

                case ReplyFormat.Paragraph:
                    return !string.IsNullOrEmpty(candidateAnswer) && !string.IsNullOrWhiteSpace(candidateAnswer);

                case ReplyFormat.Dropdown:
                    return !string.IsNullOrEmpty(candidateAnswer) && !string.IsNullOrWhiteSpace(candidateAnswer);

                case ReplyFormat.MultipleChoice:
                    return selectedItems.Count <= maxNoOfItemsToSelect && selectedItems.Count > 0;

                case ReplyFormat.Number:
                    return int.TryParse(candidateAnswer, out _) || double.TryParse(candidateAnswer, out _);
                default:
                    return false;
            }
        }
    }

}
