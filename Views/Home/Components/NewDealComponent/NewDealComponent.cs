using Tennis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Home.Components.NewDealComponent;

public class NewDealComponent : ViewComponent
{
	private readonly IProductRepository _repo;

	public NewDealComponent(IProductRepository repo)
	{
		_repo = repo;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var obj = await _repo.GetListAsync(
			x => x.ProductDateCreated.Month == DateTime.Now.Month);
		return View("NewDealComponent", obj);
	}
}