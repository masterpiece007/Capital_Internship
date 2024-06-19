using Capital_Internship.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Domain.Dtos
{
    public class CandidateApplicationDto
    {

        public CandidateApplicationDto()
        {
            QuestionResponses = new List<QuestionResponseDto>();
        }
        [Required]
        public Guid ProgramId { get; set; }
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

        public List<QuestionResponseDto>? QuestionResponses { get; set; }

        public CandidateApplication MapToCandidateApplication()
        {
            return new CandidateApplication
            {
                Program_Id = ProgramId,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Nationality = Nationality,
                CurrentResidence = CurrentResidence,
                IdNumber = IdNumber,
                DateOfBirth = DateOfBirth,
                Gender = Gender
            };
        }
    }

}
