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
            Comments = dto.Comments;
            CreateDate = DateTime.UtcNow;
        }
        // for use by EFCore
        public Feedback()
        {
        }
        public FeedbackType Type { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Comments { get; private set; }
    }

    public class FeedbackDto
    {
        [Required]
        public string Type { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(1000)]
        public string Comments { get; set; }
    }

    public enum FeedbackType
    {
        Complaint = 0,
        Suggestion = 1,
        Praise = 2
    }
}
