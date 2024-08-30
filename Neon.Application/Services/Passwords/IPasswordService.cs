namespace Neon.Application.Services.Passwords;

public interface IPasswordService
{
    public Task<string> HashAsync(string password);
    public Task<bool> CompareAsync(string hash, string password);
}