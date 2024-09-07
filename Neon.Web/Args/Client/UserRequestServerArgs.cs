namespace Neon.Web.Args.Client;

public abstract class RequesterRequestServerArgs
{
    public Guid RequesterKey { get; init; }
    public string RequesterUsername { get; init; } = null!;
}

public abstract class ResponderRequestServerArgs
{
    public Guid ResponderKey { get; init; }
    public string ResponderUsername { get; init; } = null!;
}

public abstract class UserRequestSentArgs : RequesterRequestServerArgs;
public abstract class UserRequestAcceptedArgs : ResponderRequestServerArgs;
public abstract class UserRequestDeclinedArgs : ResponderRequestServerArgs;
public abstract class UserRequestCanceledArgs : RequesterRequestServerArgs;

public class FriendRequestSentArgs : UserRequestSentArgs;
public class FriendRequestAcceptedArgs : UserRequestAcceptedArgs;
public class FriendRequestDeclinedArgs : UserRequestDeclinedArgs;
public class FriendRequestCanceledArgs : UserRequestCanceledArgs;

public class TradeRequestSentArgs : UserRequestSentArgs;
public class TradeRequestAcceptedArgs : UserRequestAcceptedArgs;
public class TradeRequestDeclinedArgs : UserRequestDeclinedArgs;
public class TradeRequestCanceledArgs : UserRequestCanceledArgs;

public class DuelRequestSentArgs : UserRequestSentArgs;
public class DuelRequestAcceptedArgs : UserRequestAcceptedArgs;
public class DuelRequestDeclinedArgs : UserRequestDeclinedArgs;
public class DuelRequestCanceledArgs : UserRequestCanceledArgs;