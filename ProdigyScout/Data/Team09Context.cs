﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Models;

namespace ProdigyScout.Data
{
    public class ProdigyScoutContext : IdentityDbContext<IdentityUser>
    {
        public ProdigyScoutContext(DbContextOptions<ProdigyScoutContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        /*public DbSet<Prospect> Prospect { get; set; } = default!;*/
        public DbSet<ProdigyScout.Models.Prospect> Prospect { get; set; } = default!;
    }
}
