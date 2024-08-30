using System.ComponentModel.DataAnnotations;
using Neon.Domain.Entities;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class LoginModel : GuestModel
{
    [Display(Prompt = nameof(Resource.User_Property_Password))]
    [DataType(DataType.Password)]
    [MinLength(User.PASSWORD_MIN_LENGTH)]
    [RegularExpression(
        User.PASSWORD_REGEX,
        ErrorMessageResourceName = nameof(Resource.Error_Validation_WeakPassword))]
    public required string Password { get; init; }
}