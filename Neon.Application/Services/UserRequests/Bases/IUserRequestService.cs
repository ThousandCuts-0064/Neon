namespace Neon.Application.Services.UserRequests.Bases;

public interface IUserRequestService
{
    public Task SendAsync(int requesterId, int responderId);
    public Task AcceptAsync(int requesterId, int responderId);
    public Task DeclineAsync(int requesterId, int responderId);
    public Task CancelAsync(int requesterId, int responderId);
}