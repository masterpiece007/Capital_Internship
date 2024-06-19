using Capital_Internship.Domain.Helpers;
using Capital_Internship.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Domain.Dtos
{
    public class AdditionalQuestionDto
    {
        public ReplyFormat ReplyFormat { get; set; }
        public string QuestionBody { get; set; }
        public int MaxNoOfChoiceAllowed { get; set; } = 1;

        // 1 to Many relationship with QuestionChoice
        public List<QuestionChoiceDto> QuestionChoices_ { get; set; }

        public AdditionalQuestion MapToAdditionalQuestion()
        {
            var question = new AdditionalQuestion()
            {
                QuestionBody = QuestionBody,
                ReplyFormat = ReplyFormat,
                MaxNoOfChoiceAllowed = MaxNoOfChoiceAllowed,
            };
            if ((ReplyFormat == ReplyFormat.MultipleChoice || ReplyFormat == ReplyFormat.Dropdown) && QuestionChoices_.Count >= 0)
            {
                var listOfChoices = new List<QuestionChoice>();
                QuestionChoices_.ForEach(a => listOfChoices.Add(a.MapToQuestionChoice()));
                question.QuestionChoices.AddRange(listOfChoices);
            }
            return question;
        }
    }
    public class QuestionChoiceDto
    {
        public string Value { get; set; }

        public QuestionChoice MapToQuestionChoice()
        {
            return new QuestionChoice { Value = Value };
        }
    }

    public class EditAdditionalQuestionDto
    {
        public EditAdditionalQuestionDto()
        {
            QuestionChoices = new List<QuestionChoiceDto>();
        }
        public ReplyFormat ReplyFormat { get; set; }
        [Required]
        public string QuestionBody { get; set; }
        public Guid ProgramId { get; set; }
        public int MaxNoOfChoiceAllowed { get; set; } = 1;

        // 1 to Many relationship with QuestionChoice
        public List<QuestionChoiceDto> QuestionChoices { get; set; }
    }
}
