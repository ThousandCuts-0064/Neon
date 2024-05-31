using System.ComponentModel.DataAnnotations;
using Neon.Web.Resources;

namespace Neon.Web.Models;

public class GuestModel
{
    [Display(Prompt = nameof(Resource.User_Property_Username))]
    [StringLength(16, MinimumLength = 4)]
    public required string Username { get; init; }

    [Display(Name = nameof(Resource.User_Property_RememberMe))]
    public bool RememberMe { get; init; }
}