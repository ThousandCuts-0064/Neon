namespace Neon.Application.Interfaces;

public interface IInitializer
{
    public ValueTask RunAsync();
}