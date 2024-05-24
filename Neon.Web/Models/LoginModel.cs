using System.ComponentModel.DataAnnotations;
using Neon.Domain.Users;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class LoginModel : IValidatableObject
{
    [Required(
        ErrorMessageResourceType = typeof(Resource),
        ErrorMessageResourceName = nameof(Resource.Error_Validation_Required))]
    [StringLength(
        maximumLength: 16,
        MinimumLength = 4,
        ErrorMessageResourceType = typeof(Resource),
        ErrorMessageResourceName = nameof(Resource.Error_Validation_StringLengthWithMin))]
    public required string Username { get; set; }

    public string? Secret { get; set; }

    [Required(
        ErrorMessageResourceType = typeof(Resource),
        ErrorMessageResourceName = nameof(Resource.Error_Validation_Required))]
    public required UserRole Role { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role != UserRole.Guest && Secret is null)
            yield return new ValidationResult(Resource.Error_Validation_Required, [nameof(Secret)]);
    }
}