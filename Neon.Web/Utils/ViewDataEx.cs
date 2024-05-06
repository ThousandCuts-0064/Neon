using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Neon.Web.Utils;

public static class ViewDataEx
{
    public static void Set<TViewData>(this ViewDataDictionary viewData, TViewData data)
        where TViewData : struct, IViewData<TViewData>

    {
        data.SaveToViewData(viewData);
    }

    public static TViewData Get<TViewData>(this ViewDataDictionary viewData)
        where TViewData : struct, IViewData<TViewData>
    {
        return TViewData.FromViewData(viewData);
    }
}
