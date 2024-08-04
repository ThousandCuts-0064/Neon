using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

        var viewData = new ViewDataDictionary<ModelExpression>(ViewContext.ViewData, For);
        var viewContext = new ViewContext(ViewContext, ViewContext.View, viewData, new StringWriter());

        await GetView("_Field").RenderAsync(viewContext);

        output.Content.AppendHtml(viewContext.Writer.ToString()!);
    }

    private IView GetView([AspMvcPartialView] string partialName)
    {
        var view = _viewEngine.GetView("Views/Shared/", partialName + ".cshtml", false);

        return view.View!;
    }
}