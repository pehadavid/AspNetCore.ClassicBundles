using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCore.ClassicBundles.TagHelpers
{
    internal class EmptyRenderer
    {
        public static Task<IViewComponentResult> GetAsync(string resName)
        {
            var commMsg = $"<!-- {resName} not found ! -->";
            IViewComponentResult empty = new HtmlContentViewComponentResult(new HtmlString(commMsg));
            return Task.FromResult<IViewComponentResult>(empty);
        }
    }
}
