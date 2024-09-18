using Neon.Application.Interfaces;
using Neon.Domain.Enums;

namespace Neon.Application.Services.Users;

public interface IUserService
{
    public Task<TUserModel> FindAsync<TUserModel>(int id) where TUserModel : IUserModel<TUserModel>;
    public Task<int> FindIdAsync(Guid key);
    public Task<string> FindUsername(int id);
    public Task<UserRole> FindRoleAsync(int id);

    public Task<IReadOnlyCollection<TModel>> FindFriendsAsync<TModel>(int id)
        where TModel : IUserModel<TModel>;

    public Task<IReadOnlyCollection<TModel>> FindIncomingUserRequests<TModel>(int id)
        where TModel : IIncomingUserRequestModel<TModel>;

    public Task<IReadOnlyCollection<TModel>> FindOutgoingUserRequests<TModel>(int id)
        where TModel : IOutgoingUserRequestModel<TModel>;

    public Task<IReadOnlyCollection<TModel>> FindOtherActiveUsersAsync<TModel>(int id)
        where TModel : IUserModel<TModel>;

    /// <returns>Old connectionId</returns>
    public Task<string?> SetActiveAsync(int id, string connectionId);

    public Task SetInactiveAsync(int id, string connectionId);
    public Task SetAllInactiveAsync(DateTime lastActiveAt);
}