using System.ComponentModel.DataAnnotations;
using Neon.Domain.Entities;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class GuestModel
{
    [Display(Prompt = nameof(Resource.User_Property_Username))]
    [StringLength(User.USERNAME_MAX_LENGTH, MinimumLength = User.USERNAME_MIN_LENGTH)]
    [RegularExpression(
        User.USERNAME_REGEX,
        ErrorMessageResourceName = nameof(Resource.Error_Authenticate_UsernameInvalidCharacters))]
    public required string Username { get; init; }

    [Display(Name = nameof(Resource.User_Property_RememberMe))]
    public bool RememberMe { get; init; }
}