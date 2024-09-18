using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Neon.Web.Utils.TagHelpers;

[HtmlTargetElement("raw", TagStructure = TagStructure.WithoutEndTag)]
public class RawTagHelper : TagHelper
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    [HtmlAttributeName("src")]
    [PathReference("~/html")]
    public required string Src { get; set; }

    public RawTagHelper(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.SelfClosing;
        output.TagName = null;

        var filePath = Path.Combine(
            _webHostEnvironment.WebRootPath,
            Src.Replace('/', Path.DirectorySeparatorChar));

        var htmlContent = await File.ReadAllTextAsync(filePath);

        output.Content.SetHtmlContent(htmlContent);
    }
}