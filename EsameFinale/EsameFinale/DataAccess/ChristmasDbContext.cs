using Microsoft.EntityFrameworkCore;
using EsameFinale.Models;

namespace EsameFinale.DataAccess
{
    public class ChristmasDbContext : DbContext
    {
        public ChristmasDbContext(DbContextOptions options)
           : base(options) { }

        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<GiftOperation> GiftOperations { get; set; }
        public DbSet<Elf> Elves { get; set; }
        public DbSet<UncleChristmas> UncleChristmas { get; set; }
    }
}
