using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Neon.Web.Utils.TagHelpers.Extended;

[HtmlTargetElement("span", Attributes = "asp-validation-for-model")]
public class BindableValidationMessageTagHelper : ValidationMessageTagHelper
{
    private bool _aspValidationForModel;

    public bool AspValidationForModel
    {
        get => _aspValidationForModel;
        set
        {
            _aspValidationForModel = value;

            if (_aspValidationForModel)
                For = ViewContext.ViewData.Model as ModelExpression;
        }
    }

    public BindableValidationMessageTagHelper(IHtmlGenerator generator) : base(generator) { }
}