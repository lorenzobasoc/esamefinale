using Microsoft.EntityFrameworkCore;
using SantaClausCrm.Models;

namespace SantaClausCrm.DataAccess
{
    public class ChristmasDbContext : DbContext
    {
        public ChristmasDbContext(DbContextOptions options)
           : base(options) { }

        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<GiftOperation> GiftOperations { get; set; }
        public DbSet<Elf> Elves { get; set; }
    }
}
