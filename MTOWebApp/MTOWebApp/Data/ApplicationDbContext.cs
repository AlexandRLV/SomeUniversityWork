using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTOWebApp.Models;
using MTOWebApp.Data;

namespace MTOWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<MTOWebApp.Data.TheoryModule> TheoryModule { get; set; }

        public DbSet<MTOWebApp.Data.Paragraph> Paragraph { get; set; }

        public DbSet<MTOWebApp.Data.TestModule> TestModule { get; set; }

        public DbSet<MTOWebApp.Data.Question> Question { get; set; }

        public DbSet<MTOWebApp.Data.QuestionAnswer> QuestionAnswer { get; set; }

        public DbSet<MTOWebApp.Data.TestScore> TestScore { get; set; }
    }
}
