using System.ComponentModel.DataAnnotations;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class RegisterModel : LoginModel
{
    [Display(Prompt = nameof(Resource.User_Property_RepeatPassword))]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public required string RepeatPassword { get; init; }
}
