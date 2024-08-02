namespace Neon.Web.Models;

public class GameplayModel
{
    public required UserModel User { get; init; }
    public required IReadOnlyCollection<OpponentModel> Opponents { get; init; }
}