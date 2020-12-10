using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Florix_Feedback.Models
{
    public class MailRoomItem
    {
        public class FileItem
        {
            public string Name { get; set; } // file name
            public string ContentType { get; set; } // file content type
            public string File { get; set; } // base64 file
        }
        public MailRoomItem(MailRoomItemDto dto)
        {
            From = dto.From;
            To = dto.To;
            Subject = dto.Subject;
            Body = dto.Body;
            Attachments = new List<FileItem>();
            if (dto.Attachments != null)
            {
                foreach (var attachment in dto.Attachments)
                {
                    using var stream = attachment.OpenReadStream();
                    using var memStream = new MemoryStream();
                    stream.CopyTo(memStream);
                    var bytes = memStream.ToArray();
                    var base64 = Convert.ToBase64String(bytes);
                    Attachments.Add(new FileItem
                    {
                        Name = attachment.FileName,
                        ContentType = attachment.ContentType,
                        File = base64
                    });
                }
            }
        }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<FileItem> Attachments { get; set; }
    }

    public class MailRoomItemDto
    {
        [Required(ErrorMessage = "From is required")]
        public string From { get; set; }

        [Required(ErrorMessage = "To is required")]
        public string To { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(1000)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(10000)]
        public string Body { get; set; }

        public List<IFormFile> Attachments { get; set; }
    }
}
