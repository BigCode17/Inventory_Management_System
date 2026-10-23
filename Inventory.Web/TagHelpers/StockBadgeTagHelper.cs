using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InventoryApp.TagHelpers
{
    [HtmlTargetElement("stock-status")]
    public class StockStatusTagHelper : TagHelper
    {
        
        public int Quantity { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            if (Quantity <= 5)
            {
                output.Attributes.SetAttribute("class", "badge bg-danger text-white p-2 fs-6");
                output.Content.SetContent($" {Quantity} ");
            }
            else
            {
                output.Attributes.SetAttribute("class", "badge bg-success text-white p-2 fs-6");
                output.Content.SetContent($" {Quantity} ");
            }
        }
    }
}