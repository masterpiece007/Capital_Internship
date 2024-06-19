using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capital_Internship.Domain.Helpers;

namespace Capital_Internship.Domain.Models
{
    public class ApplicationRequirement : BaseEntity
    {
        public ApplicationRequirement()
        {
            AdditionalQuestions = new List<AdditionalQuestion>();
        }
        public bool HideFirstName { get; set; }
        public bool HideLastName { get; set; }
        public bool HideEmail { get; set; }
        public bool HidePhoneField { get; set; }
        public bool HideNationalityField { get; set; }
        public bool HideCurrentResidenceField { get; set; }
        public bool HideIdNumberField { get; set; }
        public bool HideDateOfBirthField { get; set; }
        public bool HideGenderField { get; set; }
        public List<AdditionalQuestion>? AdditionalQuestions { get; set; }

    }

}
