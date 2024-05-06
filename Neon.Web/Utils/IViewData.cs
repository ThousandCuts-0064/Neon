using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Neon.Web.Utils;

public interface IViewData<out TViewValues> where TViewValues : struct, IViewData<TViewValues>
{
    public void SaveToViewData(ViewDataDictionary viewData);
    public static abstract TViewValues FromViewData(ViewDataDictionary viewData);
}