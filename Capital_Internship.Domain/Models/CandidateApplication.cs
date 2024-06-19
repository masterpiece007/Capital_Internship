using Capital_Internship.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Domain.Models
{
    public class CandidateApplication
    {
        public CandidateApplication()
        {
            QuestionResponses = new List<QuestionResponse>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid Program_Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public string? CurrentResidence { get; set; }
        public string? IdNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public List<QuestionResponse>? QuestionResponses { get; set; }
    }

    public class QuestionResponse
    {
        public QuestionResponse()
        {
            SelectedDropdownItems = new List<SelectedMultiChoiceItem>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AdditionaQuestionId { get; set; }
        public string QuestionBody { get; set; }
        public ReplyFormat ExpectedReplyFormat { get; set; }
        public string CandidateAnswer { get; set; }
        public DateTime DateSubmited { get; set; } = DateTime.UtcNow;
        public int MaxNoOfChoiceAllowed { get; set; } = 0;
        public List<SelectedMultiChoiceItem>? SelectedDropdownItems { get; set; }
        public Guid CandidateApplicationId { get; set; }
    }

    public class SelectedMultiChoiceItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateSubmited { get; set; } = DateTime.UtcNow;
        public string Value { get; set; }
        public Guid QuestionResponseId { get; set; }
    }


}
