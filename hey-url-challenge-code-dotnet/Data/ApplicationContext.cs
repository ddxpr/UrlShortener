using hey_url_challenge_code_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HeyUrlChallengeCodeDotnet.Data
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("ApplicationContext");
                builder.UseSqlServer(connectionString);
            }

            base.OnConfiguring(builder);
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public ApplicationContext() : base()
        {

        }

        public DbSet<Url> Urls { get; set; }
    }
}