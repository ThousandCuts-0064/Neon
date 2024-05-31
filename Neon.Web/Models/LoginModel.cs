using System.ComponentModel.DataAnnotations;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class LoginModel : GuestModel
{
    [Display(Prompt = nameof(Resource.User_Property_Password))]
    [DataType(DataType.Password)]
    [StringLength(16, MinimumLength = 4)]
    public required string Password { get; init; }
}