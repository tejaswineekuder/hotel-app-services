namespace HotelApi.Models.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

/// <summary>
/// Custom validation attribute for validating travel group IDs.
/// The ID must be 6 characters long, start with a digit from 1-9,
/// and can contain a maximum of 2 letters.
/// </summary>
/// <remarks>
/// The ID must not start with a leading zero and can have a maximum of two letters.
/// </remarks>
public class GroupIdValidationAttribute : ValidationAttribute
{
    private static readonly Regex basicPattern = new Regex(@"^[1-9][A-Za-z0-9]{5}$");
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string input = value as string;

        if (string.IsNullOrEmpty(input))
            return new ValidationResult("TravelGroupId is required.");

        // Check basic pattern
        if (!basicPattern.IsMatch(input))
            return new ValidationResult("TravelGroupId must be 6 characters, start with digit 1-9, and contain only letters and digits.");

        // Count letters
        int letterCount = 0;
        foreach (char c in input)
        {
            if (char.IsLetter(c))
                letterCount++;
        }

        if (letterCount > 2)
            return new ValidationResult("TravelGroupId can contain at most 2 letters.");

        // All good
        return ValidationResult.Success;

    }
}