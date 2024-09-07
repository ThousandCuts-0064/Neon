namespace Neon.Web.Args.Hub;

public abstract class RequesterRequestClientArgs
{
    public Guid ResponderKey { get; init; }
}

public abstract class ResponderRequestClientArgs
{
    public Guid RequesterKey { get; init; }
}

public abstract class SendUserRequestArgs : RequesterRequestClientArgs;
public abstract class AcceptUserRequestArgs : ResponderRequestClientArgs;
public abstract class DeclineUserRequestArgs : ResponderRequestClientArgs;
public abstract class CancelUserRequestArgs : RequesterRequestClientArgs;

public class SendFriendRequestArgs : SendUserRequestArgs;
public class AcceptFriendRequestArgs : AcceptUserRequestArgs;
public class DeclineFriendRequestArgs : DeclineUserRequestArgs;
public class CancelFriendRequestArgs : CancelUserRequestArgs;

public class SendTradeRequestArgs : SendUserRequestArgs;
public class AcceptTradeRequestArgs : AcceptUserRequestArgs;
public class DeclineTradeRequestArgs : DeclineUserRequestArgs;
public class CancelTradeRequestArgs : CancelUserRequestArgs;

public class SendDuelRequestArgs : SendUserRequestArgs;
public class AcceptDuelRequestArgs : AcceptUserRequestArgs;
public class DeclineDuelRequestArgs : DeclineUserRequestArgs;
public class CancelDuelRequestArgs : CancelUserRequestArgs;