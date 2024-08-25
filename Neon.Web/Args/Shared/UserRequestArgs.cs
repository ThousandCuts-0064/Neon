namespace Neon.Web.Args.Shared;

public interface IUserRequestArgs
{
    public Guid ResponderKey { get; init; }
    public string ResponderUsername { get; init; }
}

public interface ISendUserRequestArgs : IUserRequestArgs;
public interface IAcceptUserRequestArgs : IUserRequestArgs;
public interface IDeclineUserRequestArgs : IUserRequestArgs;
public interface ICancelUserRequestArgs : IUserRequestArgs;

public abstract class UserRequestArgs : IUserRequestArgs
{
    public Guid ResponderKey { get; init; }
    public string ResponderUsername { get; init; } = "";
}

public class SendFriendRequestArgs : UserRequestArgs, ISendUserRequestArgs;
public class AcceptFriendRequestArgs : UserRequestArgs, IAcceptUserRequestArgs;
public class DeclineFriendRequestArgs : UserRequestArgs, IDeclineUserRequestArgs;
public class CancelFriendRequestArgs : UserRequestArgs, ICancelUserRequestArgs;

public class SendTradeRequestArgs : UserRequestArgs, ISendUserRequestArgs;
public class AcceptTradeRequestArgs : UserRequestArgs, IAcceptUserRequestArgs;
public class DeclineTradeRequestArgs : UserRequestArgs, IDeclineUserRequestArgs;
public class CancelTradeRequestArgs : UserRequestArgs, ICancelUserRequestArgs;

public class SendDuelRequestArgs : UserRequestArgs, ISendUserRequestArgs;
public class AcceptDuelRequestArgs : UserRequestArgs, IAcceptUserRequestArgs;
public class DeclineDuelRequestArgs : UserRequestArgs, IDeclineUserRequestArgs;
public class CancelDuelRequestArgs : UserRequestArgs, ICancelUserRequestArgs;