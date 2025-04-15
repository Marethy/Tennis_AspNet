using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

namespace Tennis.Views.Blog.Components.BlogCommentComponents;

public class BlogCommentComponents(IBlogRepository repo) : ViewComponent
{
    private readonly IBlogRepository _repo = repo;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        //@await Component.InvokeAsync("BlogCommentComponents")

        var obj = await _repo.GetListAsync();
        return View("BlogCommentComponents", obj);
    }
}
