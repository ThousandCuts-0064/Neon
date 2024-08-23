namespace Neon.Application.Services.UserRequests.Bases;

public interface IUserRequestService
{
    public Task SendAsync(int requesterUserId, int responderUserId);
    public Task AcceptAsync(int requesterUserId, int responderUserId);
    public Task DeclineAsync(int requesterUserId, int responderUserId);
    public Task CancelAsync(int requesterUserId, int responderUserId);
}