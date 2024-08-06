using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Neon.Application.Services;

namespace Neon.Web.Hubs;

public class GameplayHub : Hub<IGameplayHubClient>
{
    private readonly IGameplayService _gameplayService;
    private int UserId => int.Parse(Context.UserIdentifier!);

    public GameplayHub(IGameplayService gameplayService)
    {
        _gameplayService = gameplayService;
    }

    public override async Task OnConnectedAsync()
    {
        if (await _gameplayService.TrySetUserActiveAsync(UserId, Context.ConnectionId))
            return;

        await Clients.Caller.AlreadyActive();

        Context.Abort();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _gameplayService.TrySetUserInactiveAsync(UserId, Context.ConnectionId);
    }

    public async Task HandleInput(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        var handlingTask = HandleInput(text, out var message) switch
        {
            InputResult.PlainText => Clients.All.SendMessage(message),
            InputResult.ExecutedCommand => Clients.All.ExecutedCommand(message),
            InputResult.InvalidCommand => Clients.All.InvalidCommand(message),
            _ => throw new UnreachableException()
        };

        await handlingTask;
    }

    private InputResult HandleInput(string command, out string result)
    {
        var commandSpan = command.AsSpan().Trim();

        if (commandSpan[0] != Command.INITIATOR)
        {
            result = new string(commandSpan);

            return InputResult.PlainText;
        }

        switch (commandSpan.Length)
        {
            case 1:
                result = $"Try {Commands.Help.SYNTAX}";

                return InputResult.InvalidCommand;

            case > 64:
                result = "Command too long";

                return InputResult.InvalidCommand;
        }

        Span<char> normalizedCommandSpan = stackalloc char[commandSpan.Length - 1];

        commandSpan[1..].ToLowerInvariant(normalizedCommandSpan);

        Span<Range> tokensRange = stackalloc Range[16];

        var tokensCount = ((ReadOnlySpan<char>)normalizedCommandSpan)
            .Split(tokensRange, Command.SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

        if (tokensCount == tokensRange.Length)
        {
            result = "Command too long";

            return InputResult.InvalidCommand;
        }

        switch (normalizedCommandSpan[tokensRange[0]])
        {
            case Commands.Help.NAME:
                result = Commands.Help.SYNTAX;

                return InputResult.ExecutedCommand;

            case Commands.Get.NAME:
                if (tokensCount != 2)
                {
                    result = $"Expected {Commands.Get.SYNTAX}";

                    return InputResult.InvalidCommand;
                }

                if (!Guid.TryParse(normalizedCommandSpan[tokensRange[1]], out var itemKey))
                {
                    result = string.Format(Command.ARG_WRONG_TYPE, Commands.Get.ARG_ITEM_KEY);

                    return InputResult.InvalidCommand;
                }

                result = "Success";

                _gameplayService.UserGetItem(UserId, itemKey);

                return InputResult.ExecutedCommand;

            default:
                result = $"Unknown command. Try {Commands.Help.SYNTAX}";

                return InputResult.InvalidCommand;
        }
    }

    private enum InputResult
    {
        PlainText,
        ExecutedCommand,
        InvalidCommand
    }

    private static class Command
    {
        public const char INITIATOR = '/';
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