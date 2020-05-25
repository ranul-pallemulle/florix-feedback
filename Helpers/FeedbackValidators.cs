using Florix_Feedback.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Florix_Feedback.Helpers
{
    public class RequiredIfNotAnonymous : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var feedbackDto = (FeedbackDto)validationContext.ObjectInstance;
            if (feedbackDto.Anonymous || value != null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"{validationContext.DisplayName} is required.");
        }
    }
}
