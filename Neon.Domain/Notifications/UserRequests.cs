using Neon.Domain.Notifications.Bases;

namespace Neon.Domain.Notifications;

public interface IUserRequestNotification
{
    public int RequesterUserId { get; init; }
    public int ResponderUserId { get; init; }
    public Guid ResponderKey { get; init; }
    public string ResponderUserName { get; init; }
}

public interface IUserRequestSent : IUserRequestNotification;
public interface IUserRequestAccepted : IUserRequestNotification;
public interface IUserRequestDeclined : IUserRequestNotification;
public interface IUserRequestCanceled : IUserRequestNotification;

public abstract class UserRequestNotification : Notification, IUserRequestNotification
{
    public int RequesterUserId { get; init; }
    public int ResponderUserId { get; init; }
    public Guid ResponderKey { get; init; }
    public string ResponderUserName { get; init; } = "";
}

public class FriendRequestSent : UserRequestNotification, IUserRequestSent;
public class FriendRequestAccepted : UserRequestNotification, IUserRequestAccepted;
public class FriendRequestDeclined : UserRequestNotification, IUserRequestDeclined;
public class FriendRequestCanceled : UserRequestNotification, IUserRequestCanceled;

public class DuelRequestSent : UserRequestNotification, IUserRequestSent;
public class DuelRequestAccepted : UserRequestNotification, IUserRequestAccepted;
public class DuelRequestDeclined : UserRequestNotification, IUserRequestDeclined;
public class DuelRequestCanceled : UserRequestNotification, IUserRequestCanceled;

public class TradeRequestSent : UserRequestNotification, IUserRequestSent;
public class TradeRequestAccepted : UserRequestNotification, IUserRequestAccepted;
public class TradeRequestDeclined : UserRequestNotification, IUserRequestDeclined;
public class TradeRequestCanceled : UserRequestNotification, IUserRequestCanceled;