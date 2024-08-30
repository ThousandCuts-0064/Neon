namespace Neon.Web.Args.Client;

public interface IServerUserRequestArgs
{
    public Guid RequesterKey { get; init; }
    public string RequesterUsername { get; init; }
}

public interface IUserRequestSentArgs : IServerUserRequestArgs;
public interface IUserRequestAcceptedArgs : IServerUserRequestArgs;
public interface IUserRequestDeclinedArgs : IServerUserRequestArgs;
public interface IUserRequestCanceledArgs : IServerUserRequestArgs;

public abstract class ServerUserRequestArgs : IServerUserRequestArgs
{
    public Guid RequesterKey { get; init; }
    public string RequesterUsername { get; init; } = null!;
}

public class FriendRequestSentArgs : ServerUserRequestArgs, IUserRequestSentArgs;
public class FriendRequestAcceptedArgs : ServerUserRequestArgs, IUserRequestAcceptedArgs;
public class FriendRequestDeclinedArgs : ServerUserRequestArgs, IUserRequestDeclinedArgs;
public class FriendRequestCanceledArgs : ServerUserRequestArgs, IUserRequestCanceledArgs;

public class TradeRequestSentArgs : ServerUserRequestArgs, IUserRequestSentArgs;
public class TradeRequestAcceptedArgs : ServerUserRequestArgs, IUserRequestAcceptedArgs;
public class TradeRequestDeclinedArgs : ServerUserRequestArgs, IUserRequestDeclinedArgs;
public class TradeRequestCanceledArgs : ServerUserRequestArgs, IUserRequestCanceledArgs;

public class DuelRequestSentArgs : ServerUserRequestArgs, IUserRequestSentArgs;
public class DuelRequestAcceptedArgs : ServerUserRequestArgs, IUserRequestAcceptedArgs;
public class DuelRequestDeclinedArgs : ServerUserRequestArgs, IUserRequestDeclinedArgs;
public class DuelRequestCanceledArgs : ServerUserRequestArgs, IUserRequestCanceledArgs;