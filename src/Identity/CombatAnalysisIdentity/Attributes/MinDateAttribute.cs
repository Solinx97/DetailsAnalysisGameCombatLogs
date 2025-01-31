using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Attributes;

public class MinDateAttribute : ValidationAttribute
{
    private readonly DateTimeOffset _minDate;

    public MinDateAttribute()
    {
        _minDate = DateTimeOffset.UtcNow.Date;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset > _minDate)
            {
                return new ValidationResult($"Date must be before {_minDate:yyyy-MM-dd}");
            }
        }

        return ValidationResult.Success;
    }
}
