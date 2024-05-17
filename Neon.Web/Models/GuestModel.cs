namespace Neon.Web.Models;

public class GuestModel
{
    public string Username { get; set; }
    public string Secret { get; set; }
    public bool IsUsernameTaken { get; set; }
}