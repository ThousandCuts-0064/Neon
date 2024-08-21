using System.ComponentModel;

namespace Neon.Web.ViewDatas;

public class LayoutViewData
{
    [Localizable(true)]
    public required string Title { get; init; }
}