using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

namespace Tennis.Views.Blog.Components.BlogAsideComponent;

public class BlogAsideComponent(IBlogRepository repo) : ViewComponent
{
    private readonly IBlogRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var obj = await _repo.GetListAsync();
        return View("BlogAsideComponent", obj);
    }
}
