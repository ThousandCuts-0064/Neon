using System.ComponentModel;

namespace Neon.Web.Models.Data;

public class LayoutData
{
    [Localizable(true)]
    public required string Title { get; init; }
    public string? Username { get; init; }
}