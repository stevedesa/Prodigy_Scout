using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Models;

namespace ProdigyScout.Data;

public class ProdigyScoutContext : IdentityDbContext<IdentityUser>
{
    public ProdigyScoutContext(DbContextOptions<ProdigyScoutContext> options)
            : base(options)
    {
    }

    public DbSet<Prospect> Prospect { get; set; }
    public DbSet<ComplexDetails> ComplexDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ComplexDetails>()
            .HasKey(cd => cd.ProspectId); // Define the primary key

        modelBuilder.Entity<ComplexDetails>()
            .HasOne(cd => cd.Prospect) // ComplexDetails has one Prospect
            .WithOne(p => p.ComplexDetails) // Prospect has one ComplexDetails
            .HasForeignKey<ComplexDetails>(cd => cd.ProspectId); // Define foreign key constraint

        modelBuilder.Entity<Prospect>()
            .HasOne(p => p.ComplexDetails) // Prospect has one ComplexDetails
            .WithOne(cd => cd.Prospect) // ComplexDetails has one Prospect
            .HasForeignKey<ComplexDetails>(cd => cd.ProspectId); // Define foreign key constraint
    }
}
