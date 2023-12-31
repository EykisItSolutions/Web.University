﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Web.University.TagHelpers;

[HtmlTargetElement("outputSerial", Attributes = ValueAttributeName)]
public class OutputSerialTagHelper : TagHelper
{
    private const string LabelAttributeName = "label";
    private const string ValueAttributeName = "value";
    private const string PatternAttributeName = "pattern";

    [HtmlAttributeName(LabelAttributeName)]
    public string? Label { get; set; }

    [HtmlAttributeName(ValueAttributeName)]
    public ModelExpression Value { get; set; } = null!;

    [HtmlAttributeName(PatternAttributeName)]
    public string? Pattern { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs });

        var value = encoder.Encode(Value?.Model?.ToString() ?? "");
        if (string.IsNullOrEmpty(value)) // new Serial number is not known yet. display placeholder
            value = "-- autogenerated";

        var label = encoder.Encode(Label ?? Value?.Metadata?.DisplayName ?? Value?.Name ?? "");

        var patternContent = string.IsNullOrEmpty(Pattern) ? "" :
            @$"<a data-bs-toggle='tooltip' data-bs-title='{Pattern}' 
                      href='javascript: void(0);'><img src='/img/p.png' /></a>&nbsp;";

        var labelContent = $"<label class='col-sm-3 col-form-label text-end'>{patternContent}{label}</label>";
        var valueContent = $"<div class='col-sm-9 col-form-text'>{value}</div>";

        output.TagName = "div";
        output.Attributes.Add("class", "row mb-6");

        output.Content.AppendHtml(labelContent);
        output.Content.AppendHtml(valueContent);
    }
}
