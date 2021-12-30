using ServiceGrpc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Grpc.Data
{
    public class MessagesDbContext:DbContext
    {
        private readonly IConfiguration _config;
        public MessagesDbContext(IConfiguration config)
        {
            _config = config;
            try
            {
                Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public DbSet<MessageModel> Messages {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string serverUrl;
            serverUrl = _config.GetConnectionString("MessageConnection");
            if (Directory.Exists("wwwroot") && System.IO.File.Exists("wwwroot/ip.txt"))
            {
                serverUrl = System.IO.File.ReadAllText("wwwroot/ip.txt");
            }

            optionsBuilder.UseSqlServer(serverUrl);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MessageModel>().HasKey(b => b.Id);
            builder.Entity<MessageModel>().Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}
