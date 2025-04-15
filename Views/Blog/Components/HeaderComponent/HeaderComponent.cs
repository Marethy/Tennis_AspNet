using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Blog.Components.HeaderComponent;

public class HeaderComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
