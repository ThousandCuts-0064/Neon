using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Neon.Web.Utils.Extensions;

public static class DictionaryEx
{
    public static void Set<TViewData>(this ViewDataDictionary viewData, TViewData data)
    {
        viewData[typeof(TViewData).Name] = data;
    }

    public static TViewData Get<TViewData>(this ViewDataDictionary viewData)
    {
        return (TViewData)viewData[typeof(TViewData).Name]!;
    }

    public static void Set<TTempData>(this ITempDataDictionary tempData, TTempData data)
    {
        tempData[typeof(TTempData).Name] = JsonSerializer.Serialize(data);
    }

    public static TTempData Get<TTempData>(this ITempDataDictionary tempData)
    {
        return JsonSerializer.Deserialize<TTempData>((string)tempData[typeof(TTempData).Name]!)!;
    }

    public static bool TryGet<TTempData>(this ITempDataDictionary tempData, [NotNullWhen(true)] out TTempData? data)
    {
        if (tempData.TryGetValue(typeof(TTempData).Name, out var value))
        {
            data = JsonSerializer.Deserialize<TTempData>((string)value!)!;

            return true;
        }

        data = default;

        return false;
    }
}