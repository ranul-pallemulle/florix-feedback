using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Florix_Feedback.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Webhook> Webhooks { get; set; }
        public virtual DbSet<HooklessCallback> HooklessCallbacks { get; set; }
    }
}
