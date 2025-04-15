using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Home.Components.BannerGridComponent;

public class BannerGridComponent : ViewComponent
{
	private readonly ICategoryRepository _repoCategory;

	public BannerGridComponent(ICategoryRepository repoCategory)
	{
		_repoCategory = repoCategory;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repoCategory.GetListAsync();
		return View(obj);
	}
}