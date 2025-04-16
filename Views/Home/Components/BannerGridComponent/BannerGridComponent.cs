namespace Tennis.Views.Home.Components.BannerGridComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class BannerGridComponent(ICategoryRepository repoCategory) : ViewComponent
{
    private readonly ICategoryRepository _repoCategory = repoCategory;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repoCategory.GetListAsync();
        return View(obj);
    }
}
