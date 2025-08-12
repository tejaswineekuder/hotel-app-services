using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HotelApi.Models.DTOs;
using Xunit;

namespace HotelApi.Tests.Unit.Models.DTOs;

public class AssignTravellerDtoTests
{
    [Fact]
    public void AssignTravellerDto_ValidData_ShouldPassValidation()
    {
        var dto = new AssignTravellerRequestDto
        {
            TravellerId = Guid.NewGuid(),
            RoomCode = "1234"
        };

        var validationContext = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(dto, validationContext, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void AssignTravellerDto_InvalidRoomCode_ShouldFailValidation()
    {
        var dto = new AssignTravellerRequestDto
        {
            TravellerId = Guid.NewGuid(),
            RoomCode = "12" // Invalid room code
        };

        var validationContext = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(dto, validationContext, results, true);

        Assert.False(isValid);
        Assert.Equal("Room code must be 4 digits.", results[0].ErrorMessage);
    }
}