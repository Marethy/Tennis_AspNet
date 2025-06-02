using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.Shared.Components.AccountComponent;

public class AccountComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}