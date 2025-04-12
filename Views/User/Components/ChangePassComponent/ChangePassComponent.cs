using Microsoft.AspNetCore.Mvc;

namespace FoodWebMVC.Views.User.Components.ChangePassComponent;

public class ChangePassComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		return View();
	}
}