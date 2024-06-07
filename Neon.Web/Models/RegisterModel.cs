using Neon.Web.Resources;
using System.ComponentModel.DataAnnotations;

namespace Neon.Web.Models;

public class RegisterModel : LoginModel
{
    [Display(Prompt = nameof(Resource.User_Property_RepeatPassword))]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public required string RepeatPassword { get; init; }
}
