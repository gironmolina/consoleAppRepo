using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ComicBookGalleryModel.Models;

namespace ComicBookGalleryModel
{
    public class Context : DbContext
    {
        public Context()
        {
            // TODO Create Migrations
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            //Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
            Database.SetInitializer(new DatabaseInitializer());
        }

        public DbSet<ComicBook> ComicBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            //modelBuilder.Conventions.Add(new DecimalPropertyConvention(5, 2));

            // Especify decimal length to just 1 entity
            modelBuilder.Entity<ComicBook>()
                .Property(cb => cb.AverageRating)
                .HasPrecision(5,2);
        }
    }
}
