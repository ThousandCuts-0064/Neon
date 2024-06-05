using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Neon.Web.Utils.TagHelpers.Extended;

[HtmlTargetElement("label", Attributes = "asp-for-model")]
public class BindableLabelTagHelper : LabelTagHelper
{
    private bool _aspForModel;

    public bool AspForModel
    {
        get => _aspForModel;
        set
        {
            _aspForModel = value;

            if (_aspForModel)
                For = ViewContext.ViewData.Model as ModelExpression;
        }
    }

    public BindableLabelTagHelper(IHtmlGenerator generator) : base(generator) { }
}
