using System.ComponentModel.DataAnnotations;

namespace Neon.Web.Requests;

public class AuthenticateGuestRequest
{
    [Required, StringLength(16, MinimumLength = 4)]
    public required string Username { get; set; }

    public string? Secret { get; set; }
}