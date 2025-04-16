namespace Tennis.Views.Home.Components.BannerComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class BannerComponent(IBannerRepository repo) : ViewComponent
{
    private readonly IBannerRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync(take: 3);
        return View("BannerComponent", obj);
    }
}
