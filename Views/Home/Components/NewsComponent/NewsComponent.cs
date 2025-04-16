namespace Tennis.Views.Home.Components.NewsComponent;

using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

public class NewsComponent(IBlogRepository repo) : ViewComponent
{
    private readonly IBlogRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync(take: 4);
        return View("NewsComponent", obj);
    }
}
