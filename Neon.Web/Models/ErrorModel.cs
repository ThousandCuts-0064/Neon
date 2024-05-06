namespace Neon.Web.Models;

public class ErrorModel
{
    public string? RequestId { get; set; }

    public bool HasRequestId => !string.IsNullOrEmpty(RequestId);
}
