using Neon.Application.Services.UserInputs.Enums;

namespace Neon.Application.Services.UserInputs;

public interface IUserInputService
{
    public UserInputType Handle(int userId, string input, out string message);
}