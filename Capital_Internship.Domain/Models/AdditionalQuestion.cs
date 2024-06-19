using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capital_Internship.Domain.Helpers;

namespace Capital_Internship.Domain.Models
{
    public class AdditionalQuestion : BaseEntity
    {
        public AdditionalQuestion()
        {
            QuestionChoices = new List<QuestionChoice>();
        }
        public ReplyFormat ReplyFormat { get; set; }
        public required string QuestionBody { get; set; }
        public int MaxNoOfChoiceAllowed { get; set; } = 1;

        // 1 to Many relationship with QuestionChoice
        public List<QuestionChoice> QuestionChoices { get; set; }
    }

    public class QuestionChoice : BaseEntity
    {
        public string Value { get; set; }
        public Guid AdditionalQuestionId { get; set; }
    }

}
