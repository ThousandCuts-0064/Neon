using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Neon.Web.Models;

namespace Neon.Web.Utils.TagHelpers;

public class FieldTagHelper : TagHelper
{
    private readonly ICompositeViewEngine _viewEngine;

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public ModelExpression For { get; set; } = null!;

    public FieldTagHelper(ICompositeViewEngine viewEngine)
    {
        _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        var viewData = new ViewDataDictionary<GuestModel>(ViewContext.ViewData, For.Model);

        var viewContext = new ViewContext(ViewContext, ViewContext.View, ViewContext.ViewData, new StringWriter());

        await FindView("_Field").RenderAsync(viewContext);

        output.Content.AppendHtml(viewContext.Writer.ToString()!);
    }

    private IView FindView(string partialName)
    {
        var view = _viewEngine.GetView(ViewContext.ExecutingFilePath, partialName, false);

        if (!view.Success)
            view = _viewEngine.FindView(ViewContext, partialName, false);

        return view.View!;
    }
}