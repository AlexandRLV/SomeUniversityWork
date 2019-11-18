using System.Data.Entity;

namespace WcfChat.Models
{
    public class DbChat : DbContext
    {
        public DbChat() : base("DbConnection") { }

        public DbSet<Message> Messages { get; set; }
    }
}