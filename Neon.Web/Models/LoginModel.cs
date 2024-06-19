using System.ComponentModel.DataAnnotations;
using Neon.Domain.Users;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class LoginModel : GuestModel
{
    [Display(Prompt = nameof(Resource.User_Property_Password))]
    [DataType(DataType.Password)]
    [MinLength(User.PASSWORD_MIN_LENGTH)]
    public required string Password { get; init; }
}