namespace Neon.Application;

public interface IInitializer
{
    public ValueTask RunAsync();
}