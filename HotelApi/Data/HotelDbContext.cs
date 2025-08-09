using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.Models.Entities;
namespace HotelApi.Data;

/// <summary>
/// Represents the database context for the hotel API.
/// This context manages the entities related to hotel operations such as travel groups, travellers, rooms,
/// and room reservations.
/// It configures the relationships between these entities and sets up the database schema.
/// </summary>
public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public DbSet<TravelGroup> TravelGroups { get; set; }
    public DbSet<Traveller> Travellers { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomReservation> RoomReservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelGroup>()
            .HasMany(tg => tg.Travellers)
            .WithOne(t => t.TravelGroup)
            .HasForeignKey(t => t.TravelGroupId);

        modelBuilder.Entity<Traveller>()
            .HasMany(t => t.RoomReservations)
            .WithOne(rr => rr.Traveller)
            .HasForeignKey(rr => rr.TravellerId);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.RoomReservations)
            .WithOne(rr => rr.Room)
            .HasForeignKey(rr => rr.RoomID);

    }
}
