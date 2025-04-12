using Microsoft.AspNetCore.Mvc;
using Tennis.Interfaces;

namespace Tennis.Views.Home.Components.BannerComponent;

public class BannerComponent : ViewComponent
{
	private readonly IBannerRepository _repo;

	public BannerComponent(IBannerRepository repo)
	{
		_repo = repo;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repo.GetListAsync(take: 3);
		return View("BannerComponent", obj);
	}
}