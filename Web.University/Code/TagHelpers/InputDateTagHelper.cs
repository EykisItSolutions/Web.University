﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Web.University.TagHelpers;

[HtmlTargetElement("inputDateBox", Attributes = ValueAttributeName)]
public class InputDateTagHelper(IHtmlGenerator generator) : TagHelper
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

    protected IHtmlGenerator Generator { get; } = generator;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs });

        string input = "";

        var helper = new InputTagHelper(Generator)
        {
            ViewContext = ViewContext,
            For = Value
        };

        helper.Init(context);
        helper.Process(context, output);

        using (var writer = new StringWriter())
        {
            output.TagName = "input";
            output.Attributes.Add("class", "form-control js-date-picker");

            output.WriteTo(writer, HtmlEncoder.Default);
            input = writer.ToString();
            input = input.Replace(@"class=""input-validation-error""", "");
        }

        output.Attributes.Clear();
        output.PostContent.Clear();
        output.Content.Clear();

        var patternContent = string.IsNullOrEmpty(Pattern) ? "" :
            @$"<a data-bs-toggle='tooltip' data-bs-title='{Pattern}' 
                      href='javascript: void(0);'><img src='/img/p.png' /></a>&nbsp;";

        var label = encoder.Encode(Label ?? Value?.Metadata?.DisplayName ?? Value?.Name ?? "");
        var name = encoder.Encode(Value?.Name ?? "");
        var labelContent = $"<label for='{name}' class='col-sm-3 col-form-label text-end'>{patternContent}{label}</label>";

        var validateBuilder = Generator.GenerateValidationMessage(
                ViewContext,
                Value?.ModelExplorer,
                Value?.Name,
                message: null,
                tag: null,
                htmlAttributes: null);

        var divBuilder = new TagBuilder("div");
        divBuilder.AddCssClass("col-sm-9");
        divBuilder.InnerHtml.AppendHtml(input);
        divBuilder.InnerHtml.AppendHtml(validateBuilder);

        output.TagName = "div";
        output.Attributes.Add("class", "row mb-6");

        output.Content.AppendHtml(labelContent);
        output.Content.AppendHtml(divBuilder);
    }
}
