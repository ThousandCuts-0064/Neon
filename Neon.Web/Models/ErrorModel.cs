namespace Neon.Web.Models;

public class ErrorModel
{
    public string? RequestId { get; init; }

    public bool HasRequestId => !string.IsNullOrEmpty(RequestId);
}
