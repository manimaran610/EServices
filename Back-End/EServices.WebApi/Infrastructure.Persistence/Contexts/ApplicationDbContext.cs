using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<CustomerDetail> CustomerDetails { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomGrill> RoomGrills { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Trainee> Trainees { get; set; }






        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }

            base.OnModelCreating(builder);

            builder.Entity<CustomerDetail>()
                .HasOne(e => e.Instrument)
                .WithMany(e => e.CustomerDetails)
                .HasForeignKey(e => e.InstrumentId)
                .HasPrincipalKey(e => e.Id);

            builder.Entity<Room>()
           .HasOne(e => e.CustomerDetail)
           .WithMany(e => e.Rooms)
           .HasForeignKey(e => e.CustomerDetailId)
           .HasPrincipalKey(e => e.Id);


            builder.Entity<RoomGrill>()
           .HasOne(e => e.Room)
           .WithMany(e => e.RoomGrills)
           .HasForeignKey(e => e.RoomId)
          .HasPrincipalKey(e => e.Id);


            builder.Entity<RoomLocation>()
           .HasOne(e => e.Room)
           .WithMany(e => e.RoomLocations)
           .HasForeignKey(e => e.RoomId)
          .HasPrincipalKey(e => e.Id);

            builder.Entity<Log>()
                .ToTable("Logs", t => t.ExcludeFromMigrations());

            builder.Entity<CustomerDetail>()
                .HasOne(e => e.Trainee)
                .WithMany(e => e.CustomerDetails)
                .HasForeignKey(e => e.TraineeId)
                .HasPrincipalKey(e => e.Id);





        }


    }
}
