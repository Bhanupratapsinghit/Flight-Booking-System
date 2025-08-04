
using FlightProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<BookingPassenger> BookingPassengers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Configure Airport entity (explicitly defining the key isn't necessary when using [Key] attribute,
            // but we'll keep the rest of the configuration)

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" }
            );

             modelBuilder.Entity<Flight>()
            .Property(f => f.Fare)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
            .Property(f => f.TotalFare)
            .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Airport>()
                .HasKey(a => a.AirportCode);

            // Configure relationships
            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.User)
                .WithMany(u => u.Passengers)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<BookingPassenger>()
                .HasOne(bp => bp.Booking)
                .WithMany(b => b.BookingPassengers)
                .HasForeignKey(bp => bp.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingPassenger>()
                .HasOne(bp => bp.Passenger)
                .WithMany(p => p.BookingPassengers)
                .HasForeignKey(bp => bp.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airline)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirlineId);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.OriginAirportCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationAirport)
                .WithMany(a => a.ArrivingFlights)
                .HasForeignKey(f => f.DestinationAirportCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);
        }
    }
}