namespace Neon.Domain.Users;

public interface IUserRepository
{
    public Task SetActive(int userId, bool isActive);
}