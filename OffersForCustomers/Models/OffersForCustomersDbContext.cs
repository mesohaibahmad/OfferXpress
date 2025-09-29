namespace OfferXpress.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class OfferXpressDbContext : IdentityDbContext<User>
    {
        public OfferXpressDbContext(DbContextOptions<OfferXpressDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branches> Branches { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<OfferedItemRows> OfferedItemRows { get; set; }
        public DbSet<Offers> Offers { get; set; }
        public DbSet<RutarCompany> RutarCompany { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Configure relationships
        //    modelBuilder.Entity<Branches>()
        //        .HasOne(b => b.Country)
        //        .WithMany(c => c.Branches)
        //        .HasForeignKey(b => b.CountryId);

        //    modelBuilder.Entity<Branches>()
        //        .HasOne(b => b.RutarCompany)
        //        .WithMany(rc => rc.Branches)
        //        .HasForeignKey(b => b.RutarCompanyId);

        //    modelBuilder.Entity<OfferedItemRows>()
        //        .HasOne(o => o.Offer)
        //        .WithMany(offer => offer.OfferedItemRows)
        //        .HasForeignKey(o => o.OfferId);

        //    modelBuilder.Entity<Offers>()
        //        .HasOne(o => o.Branch)
        //        .WithMany(b => b.Offers)
        //        .HasForeignKey(o => o.BranchId);
        //}

        // Optionally, you can override SaveChanges to add custom behavior if needed
        public override int SaveChanges()
        {
            // Custom logic before saving changes, if any

            return base.SaveChanges();
        }
    }

}
