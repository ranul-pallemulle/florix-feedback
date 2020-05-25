using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Florix_Feedback.Models
{
    public class Webhook : BaseEntity
    {
        public Webhook(WebhookCreationDto dto)
        {
            CallbackUrl = dto.CallbackUrl;
            FeedbackType = (FeedbackType)Enum.Parse(typeof(FeedbackType), dto.FeedbackType, true);
            SubscriptionId = Guid.NewGuid().ToString();
            CreateDate = DateTime.UtcNow;
        }
        // for use by EFCore
        public Webhook()
        {
        }
        public string CallbackUrl { get; private set; }
        public FeedbackType FeedbackType { get; private set; }
        public string SubscriptionId { get; private set; }
    }

    public class WebhookCreationDto
    {
        public string CallbackUrl { get; set; }
        public string FeedbackType { get; set; }
    }

    public class WebhookResponse
    {
        public string SubscriptionId { get; set; }
    }
}
