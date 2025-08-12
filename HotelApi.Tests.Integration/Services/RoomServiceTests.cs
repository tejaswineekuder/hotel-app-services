using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelApi.Models.DTOs;
using HotelApi.Models.Entities;
using HotelApi.Services;
using HotelApi.Interfaces;
using HotelApi.Data;
using HotelApi.Tests.Integration.Data;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Tests.Integraion.Services;

public class RoomServiceTests
{
    private readonly IRoomService _roomService;
    private readonly HotelDbContext _dbContext;    
    private readonly Guid _travellerId = Guid.NewGuid();

    public RoomServiceTests()
    {
        // Setup in-memory database or mock context
        var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseInMemoryDatabase(databaseName: "HotelApiTestDb")
            .Options;

        _dbContext = new HotelDbContext(options);
        _roomService = new RoomService(_dbContext);
        _travellerId = Guid.NewGuid();
        // Arrange
        TestDbInitializer.Initialize(_dbContext, _travellerId);

    }

    [Fact]
    public async Task GetRoomsForToday_ShouldReturnAvailableRooms()
    { 
        // Act
        var result = await _roomService.GetRoomsForToday();

        // Assert
        Assert.Single(result);
        Assert.Equal("0101", result.First().RoomCode);
    }

    [Fact]
    public async Task GetRoomsForToday_ShouldNotReturnCheckedInRooms()
    { 
        // Act
        var result = await _roomService.GetRoomsForToday();

        // Assert
        Assert.Single(result);
        Assert.Equal("0101", result.First().RoomCode);
    }

    [Fact]
    public async Task AssignTravellerToRoom_ShouldReturnFalse_WhenRoomFull()
    {
        // Act
        var result = await _roomService.AssignTravellerToRoom(_travellerId, "0101");

        // Assert
        Assert.True(string.IsNullOrEmpty(result.AssignedRoomCode));
        Assert.True(result.StatusMessage?.Contains("Room is full"));
    }


    [Fact]
    public async Task MoveTraveller_ShouldReturnTrue_WhenSuccessful()
    {
        // Act
        var result = await _roomService.MoveTraveller(_travellerId, "0101", "0102");

        // Assert
        Assert.True(result.AssignedRoomCode == "0102");
        Assert.True(result.OldRoomCode == "0101");
    }
}
