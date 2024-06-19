using Capital_Internship.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Domain.Dtos
{
    public class ProgramDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public ApplicationRequirementDto AppRequirement { get; set; }

        public Program_ MapToProgram_()
        {
            return new Program_
            {
                Title = Title,
                Description = Description
            };
        }
    }

    public class GetProgramDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
