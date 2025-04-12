using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.User.Components.ShowProfileComponent;

public class ShowProfileComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}