using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.User.Components.ChangePassComponent;

public class ChangePassComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}