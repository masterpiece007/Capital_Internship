using Capital_Internship.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital_Internship.Domain.Dtos
{
    public class ApplicationRequirementDto
    {
        public bool HidePhoneField { get; set; }
        public bool HideNationalityField { get; set; }
        public bool HideCurrentResidenceField { get; set; }
        public bool HideIdNumberField { get; set; }
        public bool HideDateOfBirthField { get; set; }
        public bool HideGenderField { get; set; }
        public List<AdditionalQuestionDto> AdditionalQuestions { get; set; }


        public ApplicationRequirement MapToApplicationRequirement()
        {
            return new ApplicationRequirement()
            {
                HideFirstName = false,
                HideLastName = false,
                HideEmail = false,
                HideCurrentResidenceField = HideCurrentResidenceField,
                HideGenderField = HideGenderField,
                HideIdNumberField = HideIdNumberField,
                HideNationalityField = HideNationalityField,
                HidePhoneField = HidePhoneField,
                HideDateOfBirthField = HideDateOfBirthField,
            };

        }

    }
}
