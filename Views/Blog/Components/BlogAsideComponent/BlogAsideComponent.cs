using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Blog.Components.BlogAsideComponent;

public class BlogAsideComponent : ViewComponent
{
	private readonly IBlogRepository _repo;

	public BlogAsideComponent(IBlogRepository repo)
	{
		_repo = repo;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repo.GetListAsync();
		return View("BlogAsideComponent", obj);
	}
}