using Neon.Application.Services.Lobbies;
using Neon.Application.Services.UserInputs.Enums;

namespace Neon.Application.Services.UserInputs;

internal class UserInputService : IUserInputService
{
    private readonly ILobbyService _lobbyService;

    public UserInputService(ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }

    public UserInputType Handle(int userId, string input, out string message)
    {
        message = "";

        if (string.IsNullOrWhiteSpace(input))
            return UserInputType.Empty;

        if (input[0] == ' ')
        {
            message = input.Trim();

            return UserInputType.PlainText;
        }

        var inputAsSpan = input.AsSpan().Trim();

        if (inputAsSpan[0] != Initiators.COMMAND)
        {
            if (inputAsSpan[0] == Initiators.IMPORTANT_TEXT)
            {
                if (inputAsSpan.Length == 1)
                    return UserInputType.Empty;

                message = new string(inputAsSpan[1..].TrimStart());

                return string.IsNullOrWhiteSpace(message)
                    ? UserInputType.Empty
                    : UserInputType.ImportantText;
            }

            message = new string(inputAsSpan);

            return UserInputType.PlainText;
        }

        switch (inputAsSpan.Length)
        {
            case 1:
                message = $"Try {Commands.Help.SYNTAX}";

                return UserInputType.InvalidCommand;

            case > 64:
                message = "Command too long";

                return UserInputType.InvalidCommand;
        }

        Span<char> normalizedCommandSpan = stackalloc char[inputAsSpan.Length - 1];

        inputAsSpan[1..].ToLowerInvariant(normalizedCommandSpan);

        Span<Range> tokensRange = stackalloc Range[16];

        var tokensCount = ((ReadOnlySpan<char>)normalizedCommandSpan)
            .Split(tokensRange, Command.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

        if (tokensCount == tokensRange.Length)
        {
            message = "Command too long";

            return UserInputType.InvalidCommand;
        }

        switch (normalizedCommandSpan[tokensRange[0]])
        {
            case Commands.Help.NAME:
                message = Commands.Help.SYNTAX;

                return UserInputType.ExecutedCommand;

            case Commands.Get.NAME:
                if (tokensCount != 2)
                {
                    message = $"Expected {Commands.Get.SYNTAX}";

                    return UserInputType.InvalidCommand;
                }

                if (!Guid.TryParse(normalizedCommandSpan[tokensRange[1]], out var itemKey))
                {
                    message = string.Format(Command.ARG_WRONG_TYPE, Commands.Get.ARG_ITEM_KEY);

                    return UserInputType.InvalidCommand;
                }

                message = "Success";

                _lobbyService.GiveItem(userId, itemKey);

                return UserInputType.ExecutedCommand;

            default:
                message = $"Unknown command. Try {Commands.Help.SYNTAX}";

                return UserInputType.InvalidCommand;
        }
    }

    private static class Initiators
    {
        public const char IMPORTANT_TEXT = '!';
        public const char COMMAND = '/';
    }

    private static class Command
    {
        public const char SEPARATOR = ' ';

        public const string ARG_WRONG_TYPE = "{0} is wrong type";
    }

    private static class Commands
    {
        private const string INITIATOR = "/";
        private const string SEPARATOR = " ";

        private static class Required
        {
            public const string L = "<";
            public const string R = ">";
        }

        private static class Optional
        {
            public const string L = "[";
            public const string R = "]";
        }

        private static class Type
        {
            public const string OF = ":";
            public const string TEXT = "text";
            public const string KEY = "key";
        }

        public static class Help
        {
            public const string NAME = "help";
            public const string ARG_COMMAND_NAME = "command-name";

            public const string SYNTAX =
                $"{INITIATOR}{NAME}{SEPARATOR}" +
                $"{Optional.L}{ARG_COMMAND_NAME}{Type.OF}{Type.TEXT}{Optional.R}";
        }

        public static class Get
        {
            public const string NAME = "get";
            public const string ARG_ITEM_KEY = "item-key";

            public const string SYNTAX =
                $"{INITIATOR}{NAME}{SEPARATOR}" +
                $"{Required.L}{ARG_ITEM_KEY}{Type.OF}{Type.KEY}{Required.R}";
        }
    }
}