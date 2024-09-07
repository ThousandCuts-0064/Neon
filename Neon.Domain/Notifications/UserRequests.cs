using Neon.Domain.Notifications.Bases;

namespace Neon.Domain.Notifications;

public abstract class UserRequestNotification : Notification
{
    public int RequesterId { get; init; }
    public Guid RequesterKey { get; init; }
    public int ResponderId { get; init; }
    public Guid ResponderKey { get; init; }
}

public abstract class RequesterRequestNotification : UserRequestNotification
{
    public string RequesterUsername { get; init; } = null!;
}

public abstract class ResponderRequestNotification : UserRequestNotification
{
    public string ResponderUsername { get; init; } = null!;
}

public abstract class UserRequestSent : RequesterRequestNotification;
public abstract class UserRequestAccepted : ResponderRequestNotification;
public abstract class UserRequestDeclined : ResponderRequestNotification;
public abstract class UserRequestCanceled : RequesterRequestNotification;

public class FriendRequestSent : UserRequestSent;
public class FriendRequestAccepted : UserRequestAccepted;
public class FriendRequestDeclined : UserRequestDeclined;
public class FriendRequestCanceled : UserRequestCanceled;

public class DuelRequestSent : UserRequestSent;
public class DuelRequestAccepted : UserRequestAccepted;
public class DuelRequestDeclined : UserRequestDeclined;
public class DuelRequestCanceled : UserRequestCanceled;

public class TradeRequestSent : UserRequestSent;
public class TradeRequestAccepted : UserRequestAccepted;
public class TradeRequestDeclined : UserRequestDeclined;
public class TradeRequestCanceled : UserRequestCanceled;