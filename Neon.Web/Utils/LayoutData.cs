using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Neon.Web.Utils;

public readonly struct LayoutData : IViewData<LayoutData>
{
    [Localizable(true)]
    public required string Title { get; init; }
    public string? Username { get; init; }

    public void SaveToViewData(ViewDataDictionary viewData)
    {
        viewData[nameof(Title)] = Title;
        viewData[nameof(Username)] = Username;
    }

    public static LayoutData FromViewData(ViewDataDictionary viewData) => new()
    {
        Title = (string?)viewData[nameof(Title)] ??
            throw new ArgumentNullException(nameof(Title), $"{nameof(LayoutData)} was not set"),
        Username = (string?)viewData[nameof(Username)]
    };
}