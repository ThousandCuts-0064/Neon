using Neon.Data.Enums;

namespace Neon.Web.Models;

public class GuestModel : UserModel
{
    public override UserRole Role => UserRole.Guest;
}