using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Florix_Feedback.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Florix_Feedback.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly ILogger<WebhooksController> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public WebhooksController(ILogger<WebhooksController> logger, IMapper mapper, AppDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WebhookCreationDto request)
        {
            try
            {
                var webhook = new Webhook(request);
                _context.Add(webhook);
                await _context.SaveChangesAsync();
                var response = new WebhookResponse
                {
                    SubscriptionId = webhook.SubscriptionId
                };
                return CreatedAtAction("Get", response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string subscriptionId)
        {
            var hook = await _context.Webhooks.Where(w => w.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
            if (hook == null)
            {
                return NotFound();
            }
            _context.Remove(hook);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> Get([FromRoute] string subscriptionId)
        {
            var hook = await _context.Webhooks.Where(w => w.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
            if (hook == null)
            {
                return NotFound();
            }
            return Ok(hook);
        }

    }

    
}