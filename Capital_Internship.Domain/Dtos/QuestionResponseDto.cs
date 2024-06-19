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
    public class QuestionResponseDto
    {
        public Guid AdditionaQuestionId { get; set; }
        [Required]
        public string QuestionBody { get; set; }
        public ReplyFormat ExpectedReplyFormat { get; set; }
        public string CandidateAnswer { get; set; }
        public int MaxNoOfChoiceAllowed { get; set; } = 0;
        public List<SelectedDropdownItemDto>? SelectedDropdownItems { get; set; }

        public QuestionResponse MapToQuestionResponse()
        {
            var mappedQuestionResponse = new QuestionResponse
            {
                AdditionaQuestionId = AdditionaQuestionId,
                QuestionBody = QuestionBody,
                ExpectedReplyFormat = ExpectedReplyFormat,
                CandidateAnswer = CandidateAnswer,
                MaxNoOfChoiceAllowed = MaxNoOfChoiceAllowed,
            };

            if (ExpectedReplyFormat == ReplyFormat.MultipleChoice && SelectedDropdownItems.Count > 0)
            {
                var listOfSelectedItem = new List<SelectedMultiChoiceItem>();
                SelectedDropdownItems.ForEach(a => listOfSelectedItem.Add(a.MapToSelectDropdownItem()));
                mappedQuestionResponse.SelectedDropdownItems.AddRange(listOfSelectedItem);
            }

            return mappedQuestionResponse;
        }
    }

    public class SelectedDropdownItemDto
    {
        public string Value { get; set; }

        public SelectedMultiChoiceItem MapToSelectDropdownItem()
        {
            return new SelectedMultiChoiceItem
            {
                Value = Value
            };
        }
    }

}
