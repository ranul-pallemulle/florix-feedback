using Florix_Feedback.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Florix_Feedback.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; protected set; }
    }
    public class Feedback : BaseEntity
    {
        public Feedback(FeedbackDto dto)
        {
            Type = (FeedbackType)Enum.Parse(typeof(FeedbackType), dto.Type, true);
            Name = dto.Name;
            Email = dto.Email;
            ReportingPersonName = dto.ReportingPersonName;
            ReportingPersonEmail = dto.ReportingPersonEmail;
            Comments = dto.Comments;
            Anonymous = dto.Anonymous;
            CreateDate = DateTime.UtcNow;
        }
        // for use by EFCore
        public Feedback()
        {
        }
        public FeedbackType Type { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string ReportingPersonName { get; private set; }
        public string ReportingPersonEmail { get; private set; }
        public string Comments { get; private set; }
        public bool Anonymous { get; private set; }
    }

    public class FeedbackDto
    {
        [Required(ErrorMessage = "Feedback type is required")]
        public string Type { get; set; }

        [RequiredIfNotAnonymous(ErrorMessage = "Name is required")]
        [StringLength(200)]
        public string Name { get; set; }

        [RequiredIfNotAnonymous(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name of reporting person is required")]
        public string ReportingPersonName { get; set; }

        [Required(ErrorMessage = "Email of reporting person is required")]
        [EmailAddress]
        public string ReportingPersonEmail { get; set; }

        [Required(ErrorMessage = "Comments are required")]
        [StringLength(1000)]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Anonymous is a required field")]
        public bool Anonymous { get; set; }
    }

    public enum FeedbackType
    {
        Complaint = 0,
        Suggestion = 1,
        Praise = 2
    }
}
