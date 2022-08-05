
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectsTrackerConsole.PL {
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext> {
        public ApplicationContext CreateDbContext(string[] args) {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
