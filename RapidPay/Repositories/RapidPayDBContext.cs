using Microsoft.EntityFrameworkCore;
using RapidPay.Models;

namespace RapidPay.Repositories
{
    public class RapidPayDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "RapidPayDB");
        }
        public DbSet<Card> Cards { get; set; }
    }
}
