using System.ComponentModel.DataAnnotations;
using Capital_Internship.Domain.Helpers;

namespace Capital_Internship.Domain.Models
{
    public class Program_ : BaseEntity
    {
        public Program_()
        {
            CandidateApplications = new List<CandidateApplication>();
        }
        public string Title { get; set; }
        public string Description { get; set; }

        //1 to 1 relationship with ApplicationRequirement
        [Required]
        public ApplicationRequirement ApplicationRequirement { get; set; } = new();

        public List<CandidateApplication>? CandidateApplications { get; set; }

    }

}
