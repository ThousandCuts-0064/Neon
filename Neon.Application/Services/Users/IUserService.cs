using Neon.Application.Models;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Users;

public interface IUserService
{
    public Task<TUserModel> FindAsync<TUserModel>(int id) where TUserModel : IUserModel<TUserModel>;
    public Task<int> FindIdAsync(Guid key);
    public Task<UserRole> FindRoleAsync(int id);

    /// <returns>Old connectionId</returns>
    public Task<string?> SetActiveAsync(int id, string connectionId);

    public Task SetInactiveAsync(int id, string connectionId);
    public Task SetAllInactiveAsync(DateTime lastActiveAt);
}