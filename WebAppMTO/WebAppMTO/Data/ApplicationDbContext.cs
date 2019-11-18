using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppMTO.Data;

namespace WebAppMTO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<TheoryModule> TheoryModule { get; set; }
        public DbSet<TestModule> TestModule { get; set; }
        public DbSet<Paragraph> Paragraph { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<WebAppMTO.Data.TestScore> TestScore { get; set; }
    }
}
