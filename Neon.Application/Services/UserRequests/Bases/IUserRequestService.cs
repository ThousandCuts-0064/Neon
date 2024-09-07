namespace Neon.Application.Services.UserRequests.Bases;

public interface IUserRequestService
{
    public Task SendAsync(int requesterId, Guid requesterKey, string requesterUsername, Guid responderKey);
    public Task AcceptAsync(Guid requesterKey, int responderId, Guid responderKey, string responderUsername);
    public Task DeclineAsync(Guid requesterKey, int responderId, Guid responderKey, string responderUsername);
    public Task CancelAsync(int requesterId, Guid requesterKey, string requesterUsername, Guid responderKey);
}