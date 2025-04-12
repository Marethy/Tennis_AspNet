using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Views.User.Components.AvatarComponent;

public class AvatarComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}