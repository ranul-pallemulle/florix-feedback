using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Florix_Feedback.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Florix_Feedback.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public FeedbackController(ILogger<FeedbackController> logger, IMapper mapper, AppDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromForm] FeedbackDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var feedback = new Feedback(feedbackDto);
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            var hooks = await _context.Webhooks.Where(w => w.FeedbackType == feedback.Type).ToListAsync();
            using (var client = new HttpClient())
            {
                foreach (var hook in hooks)
                {
                    var data = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");
                    _logger.LogInformation(
                        $"Notifying '{hook.CallbackUrl}' with type = {feedback.Type}," + $"anonymous = {feedback.Anonymous}, " +
                        $"name = {feedback.Name}, email = {feedback.Email}, reporting person = {feedback.ReportingPersonName}, " +
                        $"reporting person email = {feedback.ReportingPersonEmail}");
                    var result = await client.PostAsync(hook.CallbackUrl, data);
                    _logger.LogInformation($"Notification result: {result.StatusCode}");
                }
            }
            return RedirectToAction("Index", "Feedback");
        }
    }
}