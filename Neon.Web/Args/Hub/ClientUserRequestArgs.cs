namespace Neon.Web.Args.Hub;

public interface IClientUserRequestArgs
{
    public Guid ResponderKey { get; init; }
    public string ResponderUsername { get; init; }
}

public interface ISendUserRequestArgs : IClientUserRequestArgs;
public interface IAcceptUserRequestArgs : IClientUserRequestArgs;
public interface IDeclineUserRequestArgs : IClientUserRequestArgs;
public interface ICancelUserRequestArgs : IClientUserRequestArgs;

public abstract class ClientUserRequestArgs : IClientUserRequestArgs
{
    public Guid ResponderKey { get; init; }
    public string ResponderUsername { get; init; } = null!;
}

public class SendFriendRequestArgs : ClientUserRequestArgs, ISendUserRequestArgs;
public class AcceptFriendRequestArgs : ClientUserRequestArgs, IAcceptUserRequestArgs;
public class DeclineFriendRequestArgs : ClientUserRequestArgs, IDeclineUserRequestArgs;
public class CancelFriendRequestArgs : ClientUserRequestArgs, ICancelUserRequestArgs;

public class SendTradeRequestArgs : ClientUserRequestArgs, ISendUserRequestArgs;
public class AcceptTradeRequestArgs : ClientUserRequestArgs, IAcceptUserRequestArgs;
public class DeclineTradeRequestArgs : ClientUserRequestArgs, IDeclineUserRequestArgs;
public class CancelTradeRequestArgs : ClientUserRequestArgs, ICancelUserRequestArgs;

public class SendDuelRequestArgs : ClientUserRequestArgs, ISendUserRequestArgs;
public class AcceptDuelRequestArgs : ClientUserRequestArgs, IAcceptUserRequestArgs;
public class DeclineDuelRequestArgs : ClientUserRequestArgs, IDeclineUserRequestArgs;
public class CancelDuelRequestArgs : ClientUserRequestArgs, ICancelUserRequestArgs;