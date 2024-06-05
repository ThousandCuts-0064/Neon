using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Neon.Web.Utils.TagHelpers.Extended;

[HtmlTargetElement("input", Attributes = "asp-for-model", TagStructure = TagStructure.WithoutEndTag)]
public class BindableInputTagHelper : InputTagHelper
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

    public BindableInputTagHelper(IHtmlGenerator generator) : base(generator) { }
}