using Florix_Feedback.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Florix_Feedback.Controllers
{
    public class FloriduMailRoomController : Controller
    {
        private readonly ILogger<FloriduMailRoomController> _logger;
        private readonly AppDbContext _context;
        private readonly string _callbackKey = "MAILROOM";

        public FloriduMailRoomController(ILogger<FloriduMailRoomController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[controller]/callback")]
        [HttpGet]
        public async Task<IActionResult> GetCallbackUrl()
        {
            var cbUrl = await _context.HooklessCallbacks.Where(c => c.Name == _callbackKey).SingleOrDefaultAsync();
            if (cbUrl == null)
            {
                return NoContent();
            }
            return Ok(cbUrl.Url);
        }

        [Route("[controller]/callback")]
        [HttpPut]
        public async Task<IActionResult> ChangeCallbackUrl()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var url = await reader.ReadToEndAsync();
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }
            var cbUrl = await _context.HooklessCallbacks.Where(c => c.Name == _callbackKey).SingleOrDefaultAsync();
            if (cbUrl == null)
            {
                _context.HooklessCallbacks.Add(new HooklessCallback
                {
                    Url = url,
                    Name = _callbackKey
                });
            }
            else
            {
                cbUrl.Url = url;
                cbUrl.Name = _callbackKey;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Submit([FromForm]MailRoomItemDto mailRoomItemDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var callbackUrl = await _context.HooklessCallbacks.Where(c => c.Name == _callbackKey).SingleOrDefaultAsync();
            if (callbackUrl == null)
            {
                ModelState.AddModelError("Error", "No callback URL or multiple callback URLs found.");
                return View("Index");
            }
            var submission = new MailRoomItem(mailRoomItemDto);

            using (var client = new HttpClient())
            {
                var tempMailType = new TempMailType // for now sending a TempMailType instead of a MailRoomItem
                {
                    From = submission.From,
                    To = submission.To,
                    Subject = submission.Subject,
                    Message = submission.Body
                };
                if (submission.Attachments.Count > 0)
                {
                    tempMailType.Attachment = submission.Attachments[0].File;
                    tempMailType.AttachmentName = submission.Attachments[0].Name;
                }
                //var jsonData = new StringContent(JsonConvert.SerializeObject(submission), Encoding.UTF8, "application/json");
                var serialized = JsonConvert.SerializeObject(tempMailType, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var jsonData = new StringContent(serialized, Encoding.UTF8, "application/json");
                try
                {
                    _logger.LogInformation($"Notifying {callbackUrl.Url}");
                    var result = await client.PostAsync(callbackUrl.Url, jsonData);
                    _logger.LogInformation($"Notified");
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Failed to notify {callbackUrl}:\n{e.Message}");
                }
                
            }
            return RedirectToAction("Index", "FloriduMailRoom");
        }

        class TempMailType // can only send a single attachment for now
        {
            [JsonProperty("from")]
            public string From { get; set; }
            [JsonProperty("to")]
            public string To { get; set; }
            [JsonProperty("subject")]
            public string Subject { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("attachmentName")]
            public string AttachmentName { get; set; }
            [JsonProperty("attachment")]
            public string Attachment { get; set; }
        }
    }
}
