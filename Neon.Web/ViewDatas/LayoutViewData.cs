using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Neon.Web.ViewDatas;

public class LayoutViewData
{
    [Localizable(true)]
    public required string Title { get; init; }

    public IReadOnlyDictionary<string, string> Resource { get; init; } = ReadOnlyDictionary<string, string>.Empty;
}