namespace Neon.Application.Services.Items;

public interface IItemService
{
    public Task GiftAsync(int userId, Guid key);
}