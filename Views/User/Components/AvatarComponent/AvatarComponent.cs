using Microsoft.AspNetCore.Mvc;

namespace Tennis.Views.User.Components.AvatarComponent;

public class AvatarComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}