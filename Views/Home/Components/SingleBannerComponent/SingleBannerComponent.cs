using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

namespace Tennis.Views.Home.Components.SingleBannerComponent;

public class SingleBannerComponent(IBannerRepository repo) : ViewComponent
{
    private readonly IBannerRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetByIdAsync(6);
        return View("SingleBannerComponent", obj);
    }
}
